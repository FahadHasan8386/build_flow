using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.RemoveProjectMemberRole;

public class RemoveProjectMemberRoleResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
}
