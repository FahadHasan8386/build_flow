using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;

public class GetProjectMemberResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public ProjectMemberDetails? Member { get; set; }
}
