using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Enums;

public enum TaskStatus
{
    Todo = 1,
    InProgress = 2,
    Review = 3,
    Completed = 4,
    Cancelled = 5
}
