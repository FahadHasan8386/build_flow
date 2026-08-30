using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.UpdateComment;

public record UpdateCommentCommand(Guid CommentId,
UpdateCommentRequest Request) : IRequest<UpdateCommentResponse>;
