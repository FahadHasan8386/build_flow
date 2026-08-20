using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using BuildFlow.Domain.Enums;
using MediatR;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMember;

public class AddProjectMemberHandler
    : IRequestHandler<AddProjectMemberCommand, AddProjectMemberResponse>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddProjectMemberHandler(IProjectMemberRepository projectMemberRepository,
        IProjectRepository projectRepository,ICurrentUserService currentUserService)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AddProjectMemberResponse> Handle(AddProjectMemberCommand request,CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new AddProjectMemberResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new AddProjectMemberResponse
            {
                Success = false,
                Message = "Invalid tenant."
            };
        }

        // Check project
        var project = await _projectRepository.GetByIdAsync(
            request.Request.ProjectId,
            tenantId);

        if (project is null)
        {
            return new AddProjectMemberResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        // Check existing member
        var existingMember =
            await _projectMemberRepository.GetMemberAsync(
                request.Request.ProjectId,
                request.Request.UserId,
                tenantId);

        if (existingMember is not null)
        {
            return new AddProjectMemberResponse
            {
                Success = false,
                Message = "User is already a member of this project."
            };
        }

        // Check roles
        if (request.Request.Roles.Count == 0)
        {
            return new AddProjectMemberResponse
            {
                Success = false,
                Message = "At least one role is required."
            };
        }

        foreach (var role in request.Request.Roles.Distinct())
        {
            if (!Enum.IsDefined(
                typeof(ProjectMemberRoleType),
                role))
            {
                return new AddProjectMemberResponse
                {
                    Success = false,
                    Message = $"Invalid role: {role}."
                };
            }
        }

        // Create ProjectMember
        var member = new ProjectMember
        {
            Id = Guid.NewGuid(),
            ProjectId = request.Request.ProjectId,
            UserId = request.Request.UserId,
            TenantId = tenantId,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _currentUserService.UserId.ToString(),
            IsDeleted = false
        };

        await _projectMemberRepository.AddMemberAsync(member);

        // Add roles
        foreach (var role in request.Request.Roles.Distinct())
        {
            var memberRole = new ProjectMemberRole
            {
                Id = Guid.NewGuid(),
                ProjectMemberId = member.Id,
                Role = (ProjectMemberRoleType)role,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _currentUserService.UserId.ToString(),
                IsDeleted = false
            };

            await _projectMemberRepository.AddRoleAsync(memberRole);
        }

        return new AddProjectMemberResponse
        {
            Success = true,
            Message = "Project member added successfully.",
            ProjectMemberId = member.Id
        };
    }
}