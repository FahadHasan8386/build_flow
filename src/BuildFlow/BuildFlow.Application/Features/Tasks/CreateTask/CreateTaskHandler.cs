using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.CreateTask;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, CreateTaskResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateTaskHandler(
        ITaskRepository taskRepository,
        IProjectRepository projectRepository,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CreateTaskResponse> Handle(CreateTaskCommand request,CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var userId = _currentUserService.UserId;

        // Check project
        var project = await _projectRepository.GetByIdAsync(request.Request.ProjectId,tenantId);

        if (project is null)
        {
            return new CreateTaskResponse
            {
                Success = false,
                Message = "Project not found."
            };
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProjectId = request.Request.ProjectId,
            Title = request.Request.Title,
            Description = request.Request.Description,
            Priority = request.Request.Priority,
            Status = Domain.Enums.TaskStatus.Todo,
            DueDate = request.Request.DueDate,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId.ToString(),
            IsDeleted = false
        };

        await _taskRepository.AddAsync(task);

        return new CreateTaskResponse
        {
            Success = true,
            Message = "Task created successfully.",
            Task = task
        };
    }

}
