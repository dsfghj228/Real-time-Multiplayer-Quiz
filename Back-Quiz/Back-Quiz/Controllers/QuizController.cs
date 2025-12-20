using System.Security.Claims;
using Back_Quiz.Dtos.Quiz;
using Back_Quiz.Enums;
using Back_Quiz.MediatR.Commands;
using Back_Quiz.Quiz;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_Quiz.Controllers;

[ApiController]
[Route("api/quiz")]
public class QuizController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public QuizController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpPost("start")]
    public async Task<IActionResult> StartQuiz([FromBody] StartQuizRequestDto quiz)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized();
        
        var command = new StartQuizCommand
        {
            Category = quiz.Category,
            Difficulty = quiz.Difficulty,
            NumberOfQuestions = quiz.NumberOfQuestions,
            UserId = userId
        };
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}