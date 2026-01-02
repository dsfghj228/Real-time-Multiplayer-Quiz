using Back_Quiz.Dtos.Account;
using Back_Quiz.Interfaces;
using Back_Quiz.MediatR.Commands;
using MediatR;

namespace Back_Quiz.MediatR.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ReturnUserDto>
{
    private readonly IAccountService _accountService;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public LoginUserCommandHandler(IAccountService accountService, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _accountService = accountService;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<ReturnUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _accountService.DoesUserExist(request);

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user.Id);
        _tokenService.SetRefreshTokenCookie(refreshToken, _httpContextAccessor.HttpContext);

        return new ReturnUserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = accessToken,
        };
    }
}