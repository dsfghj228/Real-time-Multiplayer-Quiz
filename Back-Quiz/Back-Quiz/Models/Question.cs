using Back_Quiz.Enums;

namespace Back_Quiz.Models;

public class Question
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public string Category { get; set; }
    public Difficulty Difficulty { get; set; }
    
    public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();
}