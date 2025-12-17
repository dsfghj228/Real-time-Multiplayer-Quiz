namespace Back_Quiz.Models;

public class QuizTemplate
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}