using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;

public class GetProjectMembersHandler: IRequestHandler<GetProjectMembersQuery, GetProjectMembersResponse>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetProjectMembersHandler(
        IProjectMemberRepository projectMemberRepository,
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetProjectMembersResponse> Handle(
        GetProjectMembersQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new GetProjectMembersResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new GetProjectMembersResponse
            {
                Success = false,
                Message = "Invalid tenant."
            };
        }

        // Check project belongs to current tenant
        var project = await _projectRepository.GetByIdAsync(
            request.ProjectId,
            tenantId);

        if (project is null)
        {
            return new GetProjectMembersResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        // Get all members
        var members = await _projectMemberRepository
            .GetMembersByProjectAsync(
                request.ProjectId,
                tenantId);

        var result = new List<ProjectMemberItem>();


        foreach (var member in members)
        {
            var roles = await _projectMemberRepository
                .GetRolesAsync(member.Id);

            result.Add(new ProjectMemberItem
            {
                ProjectMemberId = member.Id,
                UserId = member.UserId,
                Roles = roles
                    .Select(x => x.Role)
                    .ToList()
            });
        }

        return new GetProjectMembersResponse
        {
            Success = true,
            Message = "Project members retrieved successfully.",
            Members = result
        };
    }
}