using Back_Quiz.Enums;

namespace Back_Quiz.Dtos.Quiz;

public class StartQuizRequestDto
{
    public string Category { get; set; }
    public Difficulty Difficulty { get; set; }
    public int NumberOfQuestions { get; set; }
}