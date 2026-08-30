using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetCommentById;

public record GetCommentByIdQuery(Guid CommentId) : IRequest<GetCommentByIdResponse>;
