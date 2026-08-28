using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.UpdateTask;


public record UpdateTaskCommand(Guid TaskId, UpdateTaskRequest Request) : IRequest<UpdateTaskResponse>;
