using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetCommentById;

public class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, GetCommentByIdResponse>
{
    private readonly ITaskCommentRepository _commentRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCommentByIdHandler(
        ITaskCommentRepository commentRepository,
        ICurrentUserService currentUserService)
    {
        _commentRepository = commentRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetCommentByIdResponse> Handle(
        GetCommentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var tenantId = _currentUserService.TenantId;

        var comment = await _commentRepository.GetByIdAsync(
            request.CommentId,
            tenantId);

        if (comment is null)
        {
            return new GetCommentByIdResponse
            {
                Success = false,
                Message = "Comment not found."
            };
        }

        return new GetCommentByIdResponse
        {
            Success = true,
            Message = "Comment retrieved successfully.",
            Comment = comment
        };
    }
}