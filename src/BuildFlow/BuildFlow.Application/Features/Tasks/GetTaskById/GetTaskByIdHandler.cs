using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.GetTaskById;

public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, GetTaskByIdResponse>
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTaskByIdHandler(
        ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
    {
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetTaskByIdResponse> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;

        var task = await _taskRepository.GetByIdAsync(
            request.TaskId,
            tenantId);

        if (task is null)
        {
            return new GetTaskByIdResponse
            {
                Success = false,
                Message = "Task not found."
            };
        }

        return new GetTaskByIdResponse
        {
            Success = true,
            Message = "Task retrieved successfully.",
            Task = task
        };
    }
}
