using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.RemoveProjectMemberRole;

public class RemoveProjectMemberRoleHandler
    : IRequestHandler<
        RemoveProjectMemberRoleCommand,
        RemoveProjectMemberRoleResponse>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public RemoveProjectMemberRoleHandler(
        IProjectMemberRepository projectMemberRepository,
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<RemoveProjectMemberRoleResponse> Handle(
        RemoveProjectMemberRoleCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new RemoveProjectMemberRoleResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new RemoveProjectMemberRoleResponse
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
            return new RemoveProjectMemberRoleResponse
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
            return new RemoveProjectMemberRoleResponse
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
            return new RemoveProjectMemberRoleResponse
            {
                Success = false,
                Message = "Invalid project member role."
            };
        }

        // Check role exists
        var roles = await _projectMemberRepository
            .GetRolesAsync(member.Id);

        var roleExists = roles.Any(
            x => (int)x.Role == request.Request.Role);

        if (!roleExists)
        {
            return new RemoveProjectMemberRoleResponse
            {
                Success = false,
                Message = "This role does not exist for the member."
            };
        }

        //Soft delete role
        await _projectMemberRepository.RemoveRoleAsync(
            member.Id,
            request.Request.Role);

        return new RemoveProjectMemberRoleResponse
        {
            Success = true,
            Message = "Project member role removed successfully."
        };
    }
}
