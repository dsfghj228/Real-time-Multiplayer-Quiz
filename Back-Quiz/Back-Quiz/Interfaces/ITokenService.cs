using Back_Quiz.Models;

namespace Back_Quiz.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(AppUser user);
    Task<string> GenerateRefreshTokenAsync(string userId);
    void SetRefreshTokenCookie(string refreshToken, HttpContext context);
}