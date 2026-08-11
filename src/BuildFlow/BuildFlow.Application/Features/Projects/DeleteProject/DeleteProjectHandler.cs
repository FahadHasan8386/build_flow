using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.DeleteProject;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, DeleteProjectResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteProjectHandler(
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DeleteProjectResponse> Handle(DeleteProjectCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new DeleteProjectResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new DeleteProjectResponse
            {
                Success = false,
                Message = "Tenant information is missing."
            };
        }

        var project = await _projectRepository.GetByIdAsync(request.ProjectId,
            tenantId);

        if (project is null)
        {
            return new DeleteProjectResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        await _projectRepository.DeleteAsync(
            request.ProjectId,
            tenantId);

        return new DeleteProjectResponse
        {
            Success = true,
            Message = "Project deleted successfully."
        };
    }
}
