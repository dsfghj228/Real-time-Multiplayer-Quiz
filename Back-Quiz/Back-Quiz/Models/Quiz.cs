namespace Back_Quiz.Models;

public class Quiz
{
    public Guid Id { get; set; }
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    
    public string UserId { get; set; }
    public AppUser User { get; set; }
}