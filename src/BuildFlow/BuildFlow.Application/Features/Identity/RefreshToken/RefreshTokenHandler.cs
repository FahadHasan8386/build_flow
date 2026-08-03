using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;

namespace BuildFlow.Application.Features.Identity.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var stored = await _refreshTokenRepository.GetByTokenAsync(request.Request.RefreshToken);
        if (stored is null || stored.IsExpired || stored.IsRevoked)
        {
            return new RefreshTokenResponse { Success = false, Message = "Refresh token is invalid or expired." };
        }

        var user = await _userRepository.GetByIdAsync(stored.UserId);
        if (user is null)
        {
            return new RefreshTokenResponse { Success = false, Message = "User not found." };
        }

        await _refreshTokenRepository.RevokeAsync(request.Request.RefreshToken);

        return new RefreshTokenResponse
        {
            Success = true,
            Message = "Token refreshed successfully.",
            AccessToken = _jwtTokenService.GenerateAccessToken(user, "Admin")
        };
    }
}
