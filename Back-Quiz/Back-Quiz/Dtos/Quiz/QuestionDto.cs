namespace Back_Quiz.Dtos.Quiz;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public List<QuestionOptionDto> Options { get; set; }
}