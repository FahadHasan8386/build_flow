using System.Net.Http.Headers;
using System.Net.Http.Json;
using BuildFlow.Web.Models;

namespace BuildFlow.Web.Services.Api;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> LoginAsync(LoginRequest request) => _httpClient.PostAsJsonAsync("api/auth/login", request);

    public Task<HttpResponseMessage> RegisterAsync(RegisterRequest request) => _httpClient.PostAsJsonAsync("api/auth/register-tenant", request);

    public Task<HttpResponseMessage> LogoutAsync(LogoutRequest request) => _httpClient.PostAsJsonAsync("api/auth/logout", request);

    public void SetAuthorizationToken(string? token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }
}
