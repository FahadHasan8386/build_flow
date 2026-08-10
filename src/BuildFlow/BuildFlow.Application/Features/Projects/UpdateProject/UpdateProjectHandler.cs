using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.UpdateProject;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, UpdateProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateProjectHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateProjectResponse> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new UpdateProjectResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var userId = _currentUserService.UserId;
        var tenantId = _currentUserService.TenantId;

        if (userId == Guid.Empty || tenantId == Guid.Empty)
        {
            return new UpdateProjectResponse
            {
                Success = false,
                Message = "Invalid user or tenant information."
            };
        }

        var project = await _projectRepository.GetByIdAsync(
            request.ProjectId,
            tenantId);

        if (project is null)
        {
            return new UpdateProjectResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        project.Name = request.Request.Name;
        project.Description = request.Request.Description;
        project.StartDate = request.Request.StartDate;
        project.EndDate = request.Request.EndDate;
        project.IsArchived = request.Request.IsArchived;

        project.ModifiedAt = DateTime.UtcNow;
        project.ModifiedBy = userId.ToString();

        await _projectRepository.UpdateAsync(project,tenantId);

        return new UpdateProjectResponse
        {
            Success = true,
            Message = "Project updated successfully."
        };
    }
}
