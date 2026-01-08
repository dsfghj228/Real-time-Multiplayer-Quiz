using Back_Quiz.Dtos.Quiz;
using Back_Quiz.Models;

namespace Back_Quiz.Quiz;

public class StartQuizResponse
{
    public string SessionId { get; set; }
    public QuestionDto Question { get; set; }
    public int? QuestionNumber { get; set; }
    public int? TotalQuestions { get; set; }
}