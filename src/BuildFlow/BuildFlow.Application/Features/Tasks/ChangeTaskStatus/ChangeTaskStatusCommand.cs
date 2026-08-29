using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.ChangeTaskStatus;

public record ChangeTaskStatusCommand(Guid TaskId,ChangeTaskStatusRequest Request) : IRequest<ChangeTaskStatusResponse>;
