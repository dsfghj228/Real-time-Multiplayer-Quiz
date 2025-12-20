namespace Back_Quiz.Quiz;

public class QuizSession
{
    public string UserId { get; set; }
    public List<Guid> QuestionIds { get; set; } = new();
    public int CurrentIndex { get; set; }
    public List<UserAnswer> Answers { get; set; } = new();
    public DateTime StartedAt { get; set; }
    public int TimeLimitSeconds { get; set; }
}