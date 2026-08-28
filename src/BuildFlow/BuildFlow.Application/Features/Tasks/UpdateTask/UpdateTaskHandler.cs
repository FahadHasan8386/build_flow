using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.UpdateTask;

public class UpdateTaskHandler: IRequestHandler<UpdateTaskCommand, UpdateTaskResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTaskHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateTaskResponse> Handle(UpdateTaskCommand request,CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var userId = _currentUserService.UserId;

        var task = await _taskRepository.GetByIdAsync(
            request.TaskId,
            tenantId);

        if (task is null)
        {
            return new UpdateTaskResponse
            {
                Success = false,
                Message = "Task not found."
            };
        }

        task.Title = request.Request.Title;
        task.Description = request.Request.Description;
        task.Priority = request.Request.Priority;
        task.DueDate = request.Request.DueDate;

        task.ModifiedAt = DateTime.UtcNow;
        task.ModifiedBy = userId.ToString();

        await _taskRepository.UpdateAsync(task);

        return new UpdateTaskResponse
        {
            Success = true,
            Message = "Task updated successfully.",
            Task = task
        };
    }
}