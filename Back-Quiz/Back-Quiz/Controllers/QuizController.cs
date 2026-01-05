using System.Security.Claims;
using Back_Quiz.Dtos.Quiz;
using Back_Quiz.Exceptions;
using Back_Quiz.MediatR.Commands;
using Back_Quiz.MediatR.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back_Quiz.Controllers;

[ApiController]
[Route("api/quiz")]
[Authorize]
public class QuizController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public QuizController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("start")]
    public async Task<IActionResult> StartQuiz([FromBody] StartQuizRequestDto quiz)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new CustomExceptions.UnauthorizedUsernameException();
        
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
    
    [HttpGet("{sessionId}/current")]
    public async Task<IActionResult> GetCurrentQuestion([FromRoute] string sessionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new CustomExceptions.UnauthorizedUsernameException();
        
        var query = new GetCurrentQuestionQuery
        {
            SessionId = sessionId,
            UserId = userId
        };
        
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpPost("{sessionId}/answer")]
    public async Task<IActionResult> MakeMove([FromRoute] string sessionId, [FromBody] Guid selectedOption)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new CustomExceptions.UnauthorizedUsernameException();
        
        var command = new MakeMoveCommand
        {
            SessionId = sessionId,
            UserId = userId,
            SelectedOptionId = selectedOption
        };
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("{sessionId}/finish")]
    public async Task<IActionResult> FinishQuiz([FromRoute] string sessionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new CustomExceptions.UnauthorizedUsernameException();
        
        var command = new FinishQuizCommand
        {
            SessionId = sessionId,
            UserId = userId
        };
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpGet("{sessionId}/result")]
    public async Task<IActionResult> ReturnResults([FromRoute] string sessionId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
            throw new CustomExceptions.UnauthorizedUsernameException();
        
        var query = new GetQuizResultQuery
        {
            SessionId = sessionId,
            UserId = userId
        };
        
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetQuizCategoriesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}