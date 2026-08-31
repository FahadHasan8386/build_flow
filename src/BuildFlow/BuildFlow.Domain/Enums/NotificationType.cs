using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Enums;

public enum NotificationType
{
    TaskAssigned = 1,

    TaskStatusChanged = 2,

    ProjectMemberAdded = 3,

    ProjectRoleChanged = 4,

    TaskCommentAdded = 5
}
