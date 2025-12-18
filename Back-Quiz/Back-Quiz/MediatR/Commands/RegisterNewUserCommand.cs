using Back_Quiz.Models;
using MediatR;

namespace Back_Quiz.MediatR.Commands;

public class RegisterNewUserCommand : IRequest<AppUser>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}