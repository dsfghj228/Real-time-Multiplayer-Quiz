using Back_Quiz.Dtos.Account;
using Back_Quiz.Exceptions;
using Back_Quiz.Interfaces;
using Back_Quiz.MediatR.Commands;
using Back_Quiz.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Back_Quiz.MediatR.Handlers;

public class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;

    public RegisterNewUserCommandHandler(UserManager<AppUser> userManager, IAccountService accountService, ITokenService tokenService)
    {
        _userManager = userManager;
        _accountService = accountService;
        _tokenService = tokenService;
    }
    
    public async Task Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
    {
        await _accountService.CheckUser(request);

        var appUser = new AppUser
        {
            UserName = request.Username,
            Email = request.Email
        };
        var createdUser = await _userManager.CreateAsync(appUser, request.Password);
        if (createdUser.Succeeded)
        {
            return;
        }
        var errors = string.Join(", ", createdUser.Errors.Select(e => e.Description));
        throw new CustomExceptions.InternalServerErrorException(errors);
    }
}