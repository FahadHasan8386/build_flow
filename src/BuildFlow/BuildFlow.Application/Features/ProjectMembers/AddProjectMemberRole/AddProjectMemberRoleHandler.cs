using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using BuildFlow.Domain.Enums;
using MediatR;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMemberRole;

public class AddProjectMemberRoleHandler: IRequestHandler<AddProjectMemberRoleCommand,
        AddProjectMemberRoleResponse>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddProjectMemberRoleHandler(
        IProjectMemberRepository projectMemberRepository,
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AddProjectMemberRoleResponse> Handle(AddProjectMemberRoleCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new AddProjectMemberRoleResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new AddProjectMemberRoleResponse
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
            return new AddProjectMemberRoleResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        // Check member
        var member = await _projectMemberRepository.GetMemberAsync(
            request.ProjectId,
            request.UserId,
            tenantId);

        if (member is null)
        {
            return new AddProjectMemberRoleResponse
            {
                Success = false,
                Message = "Project member not found."
            };
        }

        // Validate role
        if (!Enum.IsDefined(
            typeof(ProjectMemberRoleType),
            request.Request.Role))
        {
            return new AddProjectMemberRoleResponse
            {
                Success = false,
                Message = "Invalid project member role."
            };
        }

        var role = (ProjectMemberRoleType)request.Request.Role;

        // Check existing role
        var existingRoles =
            await _projectMemberRepository.GetRolesAsync(member.Id);

        if (existingRoles.Any(x => x.Role == role))
        {
            return new AddProjectMemberRoleResponse
            {
                Success = false,
                Message = "This role already exists for the member."
            };
        }

        // Add role
        var memberRole = new ProjectMemberRole
        {
            Id = Guid.NewGuid(),
            ProjectMemberId = member.Id,
            Role = role,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId.ToString(),
            IsDeleted = false
        };

        await _projectMemberRepository.AddRoleAsync(memberRole);

        return new AddProjectMemberRoleResponse
        {
            Success = true,
            Message = "Project member role added successfully."
        };
    }
}