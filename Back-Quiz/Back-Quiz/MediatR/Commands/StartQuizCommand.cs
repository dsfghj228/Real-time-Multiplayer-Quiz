using Back_Quiz.Enums;
using Back_Quiz.Quiz;
using MediatR;

namespace Back_Quiz.MediatR.Commands;

public class StartQuizCommand : IRequest<StartQuizResponse>
{
    public string Category { get; set; }
    public Difficulty Difficulty { get; set; }
    public int NumberOfQuestions { get; set; }
    public string UserId { get; set; }
}