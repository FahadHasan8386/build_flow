using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.UpdateTask;

public class UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public TaskPriority Priority { get; set; }

    public DateTime? DueDate { get; set; }
}
