using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMember;

public class AddProjectMemberResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public Guid ProjectMemberId { get; set; }
}
