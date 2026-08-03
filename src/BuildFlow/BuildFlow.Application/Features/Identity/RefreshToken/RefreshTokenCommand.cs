using MediatR;

namespace BuildFlow.Application.Features.Identity.RefreshToken;

public record RefreshTokenCommand(RefreshTokenRequest Request) : IRequest<RefreshTokenResponse>;
