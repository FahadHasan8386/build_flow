using BuildFlow.Domain.Common;
using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities;

public class TaskItem : BaseModel
{
    public Guid TenantId { get; set; }

    public Guid ProjectId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Todo;

    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    public Guid? AssignedToUserId { get; set; }

    public DateTime? DueDate { get; set; }
}
