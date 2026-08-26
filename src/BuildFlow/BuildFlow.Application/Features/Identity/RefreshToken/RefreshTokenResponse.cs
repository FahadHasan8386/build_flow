using BuildFlow.Shared.Responses;

namespace BuildFlow.Application.Features.Identity.RefreshToken;

public class RefreshTokenResponse : ApiResponse
{
    public string AccessToken { get; set; } = string.Empty;
}
