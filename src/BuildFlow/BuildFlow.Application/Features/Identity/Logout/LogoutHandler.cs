using BuildFlow.Application.Interfaces.Repositories;
using MediatR;

namespace BuildFlow.Application.Features.Identity.Logout;

public class LogoutHandler : IRequestHandler<LogoutCommand, LogoutResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(request.Request.RefreshToken))
        {
            await _refreshTokenRepository.RevokeAsync(request.Request.RefreshToken);
        }

        return new LogoutResponse { Success = true, Message = "Logged out successfully." };
    }
}
