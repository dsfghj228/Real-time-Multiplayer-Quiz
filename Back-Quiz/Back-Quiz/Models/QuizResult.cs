namespace Back_Quiz.Models;

public class QuizResult
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser User { get; set; }
    
    public Guid QuizTemplateId { get; set; }
    public QuizTemplate QuizTemplate { get; set; }
    
    public int CorrectAnswers { get; set; }
    public int TotalQuestions { get; set; }
    public DateTime CompletedAt { get; set; }
}