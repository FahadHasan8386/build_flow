using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BuildFlow.Web.Models;
using BuildFlow.Web.Services.Api;
using Microsoft.JSInterop;

namespace BuildFlow.Web.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ApiClient _apiClient;
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "buildflow-auth";

    public event Action? AuthChanged;

    public AuthService(ApiClient apiClient, IJSRuntime jsRuntime)
    {
        _apiClient = apiClient;
        _jsRuntime = jsRuntime;
    }

    public bool IsAuthenticated { get; private set; }
    public string? AccessToken { get; private set; }
    public string? UserEmail { get; private set; }

    public async Task InitializeAsync()
    {
        var stored = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (string.IsNullOrWhiteSpace(stored))
        {
            await ClearSessionAsync();
            return;
        }

        try
        {
            var payload = JsonSerializer.Deserialize<AuthResponse>(stored);
            if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
            {
                await ClearSessionAsync();
                return;
            }

            IsAuthenticated = true;
            AccessToken = payload.AccessToken;
            UserEmail = payload.Message;
            SetAuthorizationHeader(AccessToken);
            AuthChanged?.Invoke();
        }
        catch
        {
            await ClearSessionAsync();
        }
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var response = await _apiClient.LoginAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
        {
            return false;
        }

        await SaveSessionAsync(payload, request.Email);
        return true;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _apiClient.RegisterAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
        {
            return false;
        }

        await SaveSessionAsync(payload, request.Email);
        return true;
    }

    public async Task LogoutAsync()
    {
        var stored = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (!string.IsNullOrWhiteSpace(stored))
        {
            try
            {
                var payload = JsonSerializer.Deserialize<AuthResponse>(stored);
                if (payload is not null && !string.IsNullOrWhiteSpace(payload.RefreshToken))
                {
                    _ = await _apiClient.LogoutAsync(new LogoutRequest { RefreshToken = payload.RefreshToken });
                }
            }
            catch
            {
            }
        }

        await ClearSessionAsync();
    }

    private async Task SaveSessionAsync(AuthResponse payload, string? email = null)
    {
        var session = new AuthResponse
        {
            Success = payload.Success,
            Message = email ?? payload.Message,
            UserId = payload.UserId,
            TenantId = payload.TenantId,
            AccessToken = payload.AccessToken,
            RefreshToken = payload.RefreshToken
        };

        IsAuthenticated = true;
        AccessToken = session.AccessToken;
        UserEmail = session.Message;
        SetAuthorizationHeader(AccessToken);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(session));
        AuthChanged?.Invoke();
    }

    private async Task ClearSessionAsync()
    {
        IsAuthenticated = false;
        AccessToken = null;
        UserEmail = null;
        SetAuthorizationHeader(null);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        AuthChanged?.Invoke();
    }

    private void SetAuthorizationHeader(string? token)
    {
        _apiClient.SetAuthorizationToken(token);
    }
}
