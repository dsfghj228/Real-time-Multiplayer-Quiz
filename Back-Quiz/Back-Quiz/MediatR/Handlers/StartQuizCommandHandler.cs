using System.Security.Claims;
using Back_Quiz.Data;
using Back_Quiz.Interfaces;
using Back_Quiz.MediatR.Commands;
using Back_Quiz.Quiz;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Back_Quiz.MediatR.Handlers;

public class StartQuizCommandHandler : IRequestHandler<StartQuizCommand, StartQuizResponse>
{
    private readonly IQuizService _quizService;

    public StartQuizCommandHandler(IQuizService quizService)
    {
        _quizService = quizService;
    }
    
    public async Task<StartQuizResponse> Handle(StartQuizCommand request, CancellationToken cancellationToken)
    {
        return await _quizService.CreateQuizAsync(request.Category, request.Difficulty, request.NumberOfQuestions, request.UserId);
    }
}