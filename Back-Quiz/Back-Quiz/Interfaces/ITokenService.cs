using Back_Quiz.Models;

namespace Back_Quiz.Interfaces;

public interface ITokenService
{
    string GenerateToken(AppUser user);
}