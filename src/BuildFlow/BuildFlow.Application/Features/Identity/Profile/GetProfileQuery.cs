using MediatR;

namespace BuildFlow.Application.Features.Identity.Profile;

public record GetProfileQuery(Guid UserId) : IRequest<ProfileResponse>;
