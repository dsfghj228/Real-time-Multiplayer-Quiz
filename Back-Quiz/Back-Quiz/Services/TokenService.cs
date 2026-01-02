using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Back_Quiz.Data;
using Back_Quiz.Interfaces;
using Back_Quiz.Models;
using Microsoft.IdentityModel.Tokens;

namespace Back_Quiz.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly ApplicationDbContext _db;
    
    public TokenService(IConfiguration config, ApplicationDbContext db)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        _db = db;
    }
    
    public string GenerateAccessToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.GivenName, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = creds,
            Issuer = _config["JWT:Issuer"],
            Audience = _config["JWT:Audience"]
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateRefreshTokenAsync(string userId)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var token = Convert.ToBase64String(randomBytes);
        
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        var tokenHash = Convert.ToBase64String(bytes);
        
        var activeTokens = _db.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null);

        foreach (var refToken in activeTokens)
        {
            refToken.RevokedAt = DateTime.UtcNow;
        }
        
        var refreshToken = new RefreshToken
        {
            Token = tokenHash,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };
        
        await _db.RefreshTokens.AddAsync(refreshToken);
        await _db.SaveChangesAsync();

        return token;
    }

    public void SetRefreshTokenCookie(string refreshToken, HttpContext context)
    {
        context.Response.Cookies.Append("refresh_token", refreshToken, 
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None,
            });
    }
}