using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.AssignTask;


public record AssignTaskCommand(Guid TaskId, AssignTaskRequest Request) : IRequest<AssignTaskResponse>;
