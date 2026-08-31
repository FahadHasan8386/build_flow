using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.DeleteComment;

public record DeleteCommentCommand(Guid CommentId) : IRequest<DeleteCommentResponse>;
