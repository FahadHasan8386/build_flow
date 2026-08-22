using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.RemoveProjectMember;

public class RemoveProjectMemberResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;
}
