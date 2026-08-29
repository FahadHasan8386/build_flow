using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.DeleteTask;

public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, DeleteTaskResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTaskHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DeleteTaskResponse> Handle(
        DeleteTaskCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;

        var task = await _taskRepository.GetByIdAsync(
            request.TaskId,
            tenantId);

        if (task is null)
        {
            return new DeleteTaskResponse
            {
                Success = false,
                Message = "Task not found."
            };
        }

        await _taskRepository.DeleteAsync(
            request.TaskId,
            tenantId);

        return new DeleteTaskResponse
        {
            Success = true,
            Message = "Task deleted successfully."
        };
    }
}