using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Application.Interfaces.Services;
using BuildFlow.Domain.Enums;
using MediatR;

namespace BuildFlow.Application.Features.Tasks.ChangeTaskStatus;

public class ChangeTaskStatusHandler
    : IRequestHandler<ChangeTaskStatusCommand, ChangeTaskStatusResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public ChangeTaskStatusHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
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

        // Create notification for assigned user
        if (task.AssignedToUserId.HasValue &&
            task.AssignedToUserId.Value != userId)
        {
            await _notificationService.CreateAsync(
                tenantId,
                task.AssignedToUserId.Value,
                NotificationType.TaskStatusChanged,
                "Task Status Changed",
                $"The status of task '{task.Title}' has been changed.",
                task.Id,
                "Task",
                userId);
        }

        return new ChangeTaskStatusResponse
        {
            Success = true,
            Message = "Task status updated successfully.",
            Task = task
        };
    }
}