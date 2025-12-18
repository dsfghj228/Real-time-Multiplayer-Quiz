using Back_Quiz.Data;
using Back_Quiz.Dtos.Account;
using Back_Quiz.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Back_Quiz.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto newUser)
    {
        var command = new RegisterNewUserCommand
        {
            Username = newUser.Username,
            Email = newUser.Email,
            Password = newUser.Password
        };
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}