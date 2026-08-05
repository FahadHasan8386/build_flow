using BuildFlow.Web.Models;

namespace BuildFlow.Web.Services.Auth;

public interface IAuthService
{
    bool IsAuthenticated { get; }
    string? AccessToken { get; }
    string? UserEmail { get; }
    event Action? AuthChanged;

    Task InitializeAsync();
    Task<bool> LoginAsync(LoginRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
    Task LogoutAsync();
}
