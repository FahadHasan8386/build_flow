using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.AddComments;

public record AddCommentCommand(AddCommentRequest Request) : IRequest<AddCommentResponse>;
