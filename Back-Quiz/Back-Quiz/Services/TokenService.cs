using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Back_Quiz.Data;
using Back_Quiz.Exceptions;
using Back_Quiz.Interfaces;
using Back_Quiz.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Back_Quiz.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    
    public TokenService(IConfiguration config, ApplicationDbContext db, UserManager<AppUser> userManager)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]));
        _db = db;
        _userManager = userManager;
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
                SameSite = SameSiteMode.None
            });
    }

    public async Task LogoutAsync(HttpContext context)
    {
        if (context.Request.Cookies.TryGetValue("refresh_token", out var token))
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            var tokenHash = Convert.ToBase64String(bytes);
            
            var refreshToken = await _db.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == tokenHash && t.RevokedAt == null);
            if (refreshToken != null)
            {
                refreshToken.RevokedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
            context.Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });
        }
    }

    public async Task<string> RefreshTokenAsync(HttpContext context)
    {
        if (!context.Request.Cookies.TryGetValue("refresh_token", out var token))
            throw new CustomExceptions.SecurityTokenException("Refresh token missing");
        
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
        var tokenHash = Convert.ToBase64String(bytes);
        
        var refreshToken = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == tokenHash);
        
        if (refreshToken == null || refreshToken.ExpiresAt <= DateTime.UtcNow || refreshToken.RevokedAt != null)
            throw new CustomExceptions.SecurityTokenException("Invalid refresh token");
        
        refreshToken.RevokedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == refreshToken.UserId);
        if (user == null)
            throw new CustomExceptions.SecurityTokenException("User not found");
        
        var accessToken = GenerateAccessToken(user);
        var newRefreshToken = await GenerateRefreshTokenAsync(user.Id);
        
        SetRefreshTokenCookie(newRefreshToken, context);
        
        return accessToken;
    }
}