namespace Back_Quiz.Models;

public class QuestionOption
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    
    public Guid QuestionId { get; set; }
    public Question Question { get; set; }
}