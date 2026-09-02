using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Application.Interfaces.Services;
using BuildFlow.Domain.Enums;
using MediatR;

namespace BuildFlow.Application.Features.Tasks.AssignTask;

public class AssignTaskHandler
    : IRequestHandler<AssignTaskCommand, AssignTaskResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectMemberRepository _projectMemberRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationService _notificationService;

    public AssignTaskHandler(
        ITaskRepository taskRepository,
        IProjectMemberRepository projectMemberRepository,
        ICurrentUserService currentUserService,
        INotificationService notificationService)
    {
        _taskRepository = taskRepository;
        _projectMemberRepository = projectMemberRepository;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
    }

    public async Task<AssignTaskResponse> Handle(
        AssignTaskCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var currentUserId = _currentUserService.UserId;

        // Get task
        var task = await _taskRepository.GetByIdAsync(
            request.TaskId,
            tenantId);

        if (task is null)
        {
            return new AssignTaskResponse
            {
                Success = false,
                Message = "Task not found."
            };
        }

        // Check assigned user is a project member
        var member = await _projectMemberRepository.GetMemberAsync(
            task.ProjectId,
            request.Request.UserId,
            tenantId);

        if (member is null)
        {
            return new AssignTaskResponse
            {
                Success = false,
                Message = "User is not a member of this project."
            };
        }

        // Assign task
        task.AssignedToUserId = request.Request.UserId;

        task.ModifiedAt = DateTime.UtcNow;
        task.ModifiedBy = currentUserId.ToString();

        await _taskRepository.UpdateAsync(task);

        // Create notification
        await _notificationService.CreateAsync(
            tenantId,
            request.Request.UserId,
            NotificationType.TaskAssigned,
            "New Task Assigned",
            $"You have been assigned task: {task.Title}",
            task.Id,
            "Task",
            currentUserId);

        return new AssignTaskResponse
        {
            Success = true,
            Message = "Task assigned successfully.",
            Task = task
        };
    }
}