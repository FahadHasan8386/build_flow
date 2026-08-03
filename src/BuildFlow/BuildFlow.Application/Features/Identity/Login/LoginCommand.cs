using MediatR;

namespace BuildFlow.Application.Features.Identity.Login;

public record LoginCommand(LoginRequest Request) : IRequest<LoginResponse>;
