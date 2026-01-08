using Back_Quiz.Interfaces;
using Back_Quiz.MediatR.Commands;
using MediatR;

namespace Back_Quiz.MediatR.Handlers;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;
    
    public LogoutUserCommandHandler(IHttpContextAccessor httpContextAccessor, ITokenService tokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
    }
    
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await  _tokenService.LogoutAsync(_httpContextAccessor.HttpContext);
    }
}