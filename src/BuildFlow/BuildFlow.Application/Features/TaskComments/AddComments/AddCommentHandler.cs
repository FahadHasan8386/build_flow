using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.AddComments;

public class AddCommentHandler : IRequestHandler<AddCommentCommand, AddCommentResponse>
{
    private readonly ITaskCommentRepository _commentRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ICurrentUserService _currentUserService;

    public AddCommentHandler( ITaskCommentRepository commentRepository,ITaskRepository taskRepository,
        ICurrentUserService currentUserService)
    {
        _commentRepository = commentRepository;
        _taskRepository = taskRepository;
        _currentUserService = currentUserService;
    }

    public async Task<AddCommentResponse> Handle(AddCommentCommand request,CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var userId = _currentUserService.UserId;

        // Check Task
        var task = await _taskRepository.GetByIdAsync(
            request.Request.TaskId,
            tenantId);

        if (task is null)
        {
            return new AddCommentResponse
            {
                Success = false,
                Message = "Task not found."
            };
        }

        // Create Comment
        var comment = new TaskComment
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            TaskId = task.Id,
            UserId = userId,
            Comment = request.Request.Comment.Trim(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId.ToString(),
            IsDeleted = false
        };

        await _commentRepository.AddAsync(comment);

        return new AddCommentResponse
        {
            Success = true,
            Message = "Comment added successfully.",
            Comment = comment
        };
    }
}
