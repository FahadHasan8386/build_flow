using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.ChangeTaskStatus;

public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand, ChangeTaskStatusResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public ChangeTaskStatusHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ChangeTaskStatusResponse> Handle(
        ChangeTaskStatusCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var userId = _currentUserService.UserId;

        var task = await _taskRepository.GetByIdAsync(
            request.TaskId,
            tenantId);

        if (task is null)
        {
            return new ChangeTaskStatusResponse
            {
                Success = false,
                Message = "Task not found."
            };
        }

        task.Status = (Domain.Enums.TaskStatus)request.Request.Status;
        task.ModifiedAt = DateTime.UtcNow;
        task.ModifiedBy = userId.ToString();

        await _taskRepository.UpdateAsync(task);

        return new ChangeTaskStatusResponse
        {
            Success = true,
            Message = "Task status updated successfully.",
            Task = task
        };
    }
}
