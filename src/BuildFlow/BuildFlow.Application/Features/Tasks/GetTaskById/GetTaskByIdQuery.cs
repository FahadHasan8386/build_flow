using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.GetTaskById;

public record GetTaskByIdQuery(Guid TaskId) : IRequest<GetTaskByIdResponse>;
