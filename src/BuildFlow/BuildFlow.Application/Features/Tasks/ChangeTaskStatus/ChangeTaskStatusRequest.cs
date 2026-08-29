using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Tasks.ChangeTaskStatus;

public class ChangeTaskStatusRequest
{
    public TaskStatus Status { get; set; }
}
