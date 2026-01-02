using Back_Quiz.Data;
using Back_Quiz.Dtos.Account;
using Back_Quiz.MediatR.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        
        await _mediator.Send(command);
        return StatusCode(201);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
    {
        var command = new LoginUserCommand
        {
            Password = loginUser.Password,
            Username = loginUser.Username
        };
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutUserCommand();
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var command = new RefreshTokenCommand();
        
        var result = await _mediator.Send(command);
        return Ok(new { accessToken = result });
    }
}