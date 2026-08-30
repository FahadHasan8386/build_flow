using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.UpdateComment;

public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, UpdateCommentResponse>
{
    private readonly ITaskCommentRepository _commentRepository;
    private readonly ICurrentUserService _currentUserService;

    public UpdateCommentHandler(
        ITaskCommentRepository commentRepository,
        ICurrentUserService currentUserService)
    {
        _commentRepository = commentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<UpdateCommentResponse> Handle(
        UpdateCommentCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var userId = _currentUserService.UserId;

        var comment = await _commentRepository.GetByIdAsync(
            request.CommentId,
            tenantId);

        if (comment is null)
        {
            return new UpdateCommentResponse
            {
                Success = false,
                Message = "Comment not found."
            };
        }

        // Only comment owner can update
        if (comment.UserId != userId)
        {
            return new UpdateCommentResponse
            {
                Success = false,
                Message = "You are not allowed to update this comment."
            };
        }

        comment.Comment = request.Request.Comment.Trim();
        comment.ModifiedAt = DateTime.UtcNow;
        comment.ModifiedBy = userId.ToString();

        await _commentRepository.UpdateAsync(comment);

        return new UpdateCommentResponse
        {
            Success = true,
            Message = "Comment updated successfully.",
            Comment = comment
        };
    }
}