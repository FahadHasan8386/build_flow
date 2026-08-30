using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.TaskComments.GetComments;

public record GetCommentsQuery(Guid TaskId) : IRequest<GetCommentsResponse>;
