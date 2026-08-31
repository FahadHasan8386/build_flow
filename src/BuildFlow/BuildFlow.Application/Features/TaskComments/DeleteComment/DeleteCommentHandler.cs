using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.DeleteComment;

public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, DeleteCommentResponse>
{
    private readonly ITaskCommentRepository _commentRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteCommentHandler(
        ITaskCommentRepository commentRepository,
        ICurrentUserService currentUserService)
    {
        _commentRepository = commentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DeleteCommentResponse> Handle(
        DeleteCommentCommand request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;
        var userId = _currentUserService.UserId;

        var comment = await _commentRepository.GetByIdAsync(
            request.CommentId,
            tenantId);

        if (comment is null)
        {
            return new DeleteCommentResponse
            {
                Success = false,
                Message = "Comment not found."
            };
        }

        // Only comment owner can delete
        if (comment.UserId != userId)
        {
            return new DeleteCommentResponse
            {
                Success = false,
                Message = "You are not allowed to delete this comment."
            };
        }

        await _commentRepository.DeleteAsync(
            request.CommentId,
            tenantId);

        return new DeleteCommentResponse
        {
            Success = true,
            Message = "Comment deleted successfully."
        };
    }
}
