using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.GetTasks;

public class GetTasksHandler : IRequestHandler<GetTasksQuery, GetTasksResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTasksHandler(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetTasksResponse> Handle(GetTasksQuery request,CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;

        // Check project belongs to current tenant
        var project = await _projectRepository.GetByIdAsync(
            request.ProjectId,
            tenantId);

        if (project is null)
        {
            return new GetTasksResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        var tasks = await _taskRepository.GetByProjectAsync(
            request.ProjectId,
            tenantId);

        return new GetTasksResponse
        {
            Success = true,
            Message = "Tasks retrieved successfully.",
            Tasks = tasks
        };
    }
}
