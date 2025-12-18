using Back_Quiz.Exceptions;
using Back_Quiz.Interfaces;
using Back_Quiz.MediatR.Commands;
using Back_Quiz.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Back_Quiz.MediatR.Handlers;

public class RegisterNewUserCommandHandler : IRequestHandler<RegisterNewUserCommand, AppUser>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAccountService _accountService;

    public RegisterNewUserCommandHandler(UserManager<AppUser> userManager, IAccountService accountService)
    {
        _userManager = userManager;
        _accountService = accountService;
    }
    
    public async Task<AppUser> Handle(RegisterNewUserCommand request, CancellationToken cancellationToken)
    {
        await _accountService.CheckUser(request);
        
        var appUser = new AppUser
        {
            UserName = request.Username,
            Email = request.Email
        };
        
        var result = await _userManager.CreateAsync(appUser, request.Password);
        if (!result.Succeeded)
        {
            throw new CustomExceptions.InternalServerErrorException();
        }
        
        return appUser;
    }
}