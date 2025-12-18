using Back_Quiz.Enums;

namespace Back_Quiz.Models;

public class QuizTemplate
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    
    public string Category { get; set; }
    public Difficulty Difficulty { get; set; }
    public int QuestionCount { get; set; }
    public int TimeLimitSeconds { get; set; }
}