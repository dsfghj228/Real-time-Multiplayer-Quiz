namespace Back_Quiz.Dtos.Quiz;

public class QuestionOptionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
}