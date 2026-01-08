namespace Back_Quiz.Models;

public class RefreshToken
{
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}