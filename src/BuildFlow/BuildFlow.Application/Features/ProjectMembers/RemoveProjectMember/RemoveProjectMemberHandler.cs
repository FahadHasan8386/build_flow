using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.RemoveProjectMember;

public class RemoveProjectMemberHandler: IRequestHandler<RemoveProjectMemberCommand,RemoveProjectMemberResponse>
{
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public RemoveProjectMemberHandler(IProjectMemberRepository projectMemberRepository,
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectMemberRepository = projectMemberRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<RemoveProjectMemberResponse> Handle(
        RemoveProjectMemberCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new RemoveProjectMemberResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new RemoveProjectMemberResponse
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
            return new RemoveProjectMemberResponse
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
            return new RemoveProjectMemberResponse
            {
                Success = false,
                Message = "Project member not found."
            };
        }

        // Soft delete member
        await _projectMemberRepository.RemoveMemberAsync(
            request.ProjectId,
            request.UserId,
            tenantId);

        return new RemoveProjectMemberResponse
        {
            Success = true,
            Message = "Project member removed successfully."
        };
    }
}
