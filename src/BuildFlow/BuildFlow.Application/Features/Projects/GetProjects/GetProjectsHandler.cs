using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace BuildFlow.Application.Features.Projects.GetProjects;

public class GetProjectsHandler : IRequestHandler<GetProjectsQuery, GetProjectsResponse>
{
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetProjectsHandler( IProjectRepository projectRepository,ICurrentUserService currentUserService)
    {
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetProjectsResponse> Handle(GetProjectsQuery request,CancellationToken cancellationToken)
    {
        if(!_currentUserService.IsAuthenticated)
        {
            return new GetProjectsResponse
            {
                Success = false,
                Message = "User is not Authenticated."
            };
        }
        var  tenantId = _currentUserService.TenantId;
        if (tenantId == Guid.Empty)
        {
            return new GetProjectsResponse
            {
                Success = false,
                Message = "Tenant information is missing."
            };
        }

        var projects = await _projectRepository.GetByTenantAsync(tenantId);

        var projectData = projects.Select(project => new ProjectData
        {
            Id = project.Id,
            TenantId = project.TenantId,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            IsArchived = project.IsArchived
        }).ToList();

        return new GetProjectsResponse
        {
            Success = true,
            Message = "Projects retrieved successfully.",
            Projects = projectData
        };
    }
}
