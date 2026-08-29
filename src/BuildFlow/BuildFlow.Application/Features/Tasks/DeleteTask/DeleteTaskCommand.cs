using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.DeleteTask;

public record DeleteTaskCommand(Guid TaskId) : IRequest<DeleteTaskResponse>;
