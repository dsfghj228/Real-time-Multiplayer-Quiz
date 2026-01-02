using Back_Quiz.Interfaces;
using Back_Quiz.MediatR.Commands;
using MediatR;

namespace Back_Quiz.MediatR.Handlers;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, string>
{
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RefreshTokenCommandHandler(ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<string> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _tokenService.RefreshTokenAsync(_httpContextAccessor.HttpContext);
    }
}