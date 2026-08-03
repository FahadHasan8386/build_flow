using MediatR;

namespace BuildFlow.Application.Features.Identity.Logout;

public record LogoutCommand(LogoutRequest Request) : IRequest<LogoutResponse>;
