using Microsoft.AspNetCore.Identity;

namespace Back_Quiz.Models;

public class AppUser : IdentityUser
{
    public ICollection<QuizResult> QuizResults { get; set; } = new List<QuizResult>();
}