using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using BuildFlow.Web.Models;
using Microsoft.JSInterop;

namespace BuildFlow.Web.Services;

public class AuthStateService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "buildflow-auth";

    public event Action? AuthChanged;

    public AuthStateService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public bool IsAuthenticated { get; private set; }
    public string? AccessToken { get; private set; }
    public string? UserEmail { get; private set; }

    public async Task InitializeAsync()
    {
        var saved = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (string.IsNullOrWhiteSpace(saved))
        {
            await ClearAsync();
            return;
        }

        try
        {
            var payload = JsonSerializer.Deserialize<AuthResponse>(saved);
            IsAuthenticated = !string.IsNullOrWhiteSpace(payload?.AccessToken);
            AccessToken = payload?.AccessToken;
            UserEmail = payload?.Message;
            if (IsAuthenticated)
            {
                SetAuthorizationHeader(AccessToken);
            }
            else
            {
                await ClearAsync();
                return;
            }
            AuthChanged?.Invoke();
        }
        catch
        {
            await ClearAsync();
        }
    }

    public async Task<bool> LoginAsync(LoginRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
        {
            return false;
        }

        await SaveAsync(payload, request.Email);
        return true;
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register-tenant", request);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        var payload = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
        {
            return false;
        }

        await SaveAsync(payload, request.Email);
        return true;
    }

    public async Task LogoutAsync()
    {
        var payload = await LoadAsync();
        if (payload is not null && !string.IsNullOrWhiteSpace(payload.RefreshToken))
        {
            _ = await _httpClient.PostAsJsonAsync("api/auth/logout", new LogoutRequest { RefreshToken = payload.RefreshToken });
        }

        await ClearAsync();
    }

    private async Task SaveAsync(AuthResponse payload, string? email = null)
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

    private async Task ClearAsync()
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
        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    private async Task<AuthResponse?> LoadAsync()
    {
        var saved = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (string.IsNullOrWhiteSpace(saved))
        {
            return null;
        }

        return JsonSerializer.Deserialize<AuthResponse>(saved);
    }
}
