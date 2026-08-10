using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Projects.GetProjectById;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery , GetProjectByIdResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetProjectByIdHandler(IProjectRepository projectRepository , ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetProjectByIdResponse> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return new GetProjectByIdResponse
            {
                Success = false,
                Message = "User is not authenticated."
            };
        }

        var tenantId = _currentUserService.TenantId;

        if (tenantId == Guid.Empty)
        {
            return new GetProjectByIdResponse
            {
                Success = false,
                Message = "Tenant information id miissing"
            };
        }
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, tenantId);

        if(project == null) 
        {
            return new GetProjectByIdResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }
        return new GetProjectByIdResponse
        {
            Success = true,
            Message = "Project retrived successfully.",
            Project = new ProjectData
            {
                Id = project.Id,
                TenantId = project.TenantId,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                IsArchived = project.IsArchived
            }
        };

    }
}
