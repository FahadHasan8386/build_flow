using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.GetTasks;

public record GetTasksQuery(Guid ProjectId) : IRequest<GetTasksResponse>;
