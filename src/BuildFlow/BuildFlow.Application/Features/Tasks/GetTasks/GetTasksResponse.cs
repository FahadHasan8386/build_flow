using BuildFlow.Domain.Entities;
using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.GetTasks;

public class GetTasksResponse :ApiResponse
{
    public IEnumerable<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
