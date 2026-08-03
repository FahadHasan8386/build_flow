using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;

namespace BuildFlow.Application.Features.Identity.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginHandler(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Request.Email);
        if (user is null || !_passwordHasher.VerifyPassword(request.Request.Password, user.PasswordHash))
        {
            return new LoginResponse { Success = false, Message = "Invalid email or password." };
        }

        var role = "Admin";
        var refreshToken = _jwtTokenService.GenerateRefreshToken(user.Id);
        await _refreshTokenRepository.CreateAsync(refreshToken, null!, null!);

        return new LoginResponse
        {
            Success = true,
            Message = "Login successful.",
            UserId = user.Id,
            TenantId = user.TenantId,
            AccessToken = _jwtTokenService.GenerateAccessToken(user, role),
            RefreshToken = refreshToken.Token
        };
    }
}
