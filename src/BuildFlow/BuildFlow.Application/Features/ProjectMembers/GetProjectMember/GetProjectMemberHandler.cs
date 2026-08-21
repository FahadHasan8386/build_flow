using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMember;

public class GetProjectMemberHandler
    : IRequestHandler<GetProjectMemberQuery, GetProjectMemberResponse>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetProjectMemberHandler(
        IProjectMemberRepository projectMemberRepository,
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetProjectMemberResponse> Handle(
        GetProjectMemberQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new GetProjectMemberResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new GetProjectMemberResponse
            {
                Success = false,
                Message = "Invalid tenant."
            };
        }

        // Check project
        var project = await _projectRepository.GetByIdAsync(
            request.ProjectId,
            tenantId);

        if (project is null)
        {
            return new GetProjectMemberResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        // Get member
        var member = await _projectMemberRepository.GetMemberAsync(
            request.ProjectId,
            request.UserId,
            tenantId);

        if (member is null)
        {
            return new GetProjectMemberResponse
            {
                Success = false,
                Message = "Project member not found."
            };
        }

        // Get roles
        var roles = await _projectMemberRepository.GetRolesAsync(
            member.Id);

        return new GetProjectMemberResponse
        {
            Success = true,
            Message = "Project member retrieved successfully.",
            Member = new ProjectMemberDetails
            {
                ProjectMemberId = member.Id,
                UserId = member.UserId,
                ProjectId = member.ProjectId,
                Roles = roles
                    .Select(x => x.Role)
                    .ToList()
            }
        };
    }
}