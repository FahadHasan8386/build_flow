using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.CreateTask;

public record CreateTaskCommand(CreateTaskRequest Request) : IRequest<CreateTaskResponse>;
