using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetComments;

public class GetCommentsHandler : IRequestHandler<GetCommentsQuery, GetCommentsResponse>
{
    private readonly ITaskCommentRepository _commentRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCommentsHandler(ITaskCommentRepository commentRepository,ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
    {
        _commentRepository = commentRepository;
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetCommentsResponse> Handle(
        GetCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;

        // Check task exists in current tenant
        var task = await _taskRepository.GetByIdAsync(
            request.TaskId,
            tenantId);

        if (task is null)
        {
            return new GetCommentsResponse
            {
                Success = false,
                Message = "Task not found."
            };
        }

        var comments = await _commentRepository.GetByTaskAsync(
            request.TaskId,
            tenantId);

        return new GetCommentsResponse
        {
            Success = true,
            Message = "Comments retrieved successfully.",
            Comments = comments
        };
    }
}
