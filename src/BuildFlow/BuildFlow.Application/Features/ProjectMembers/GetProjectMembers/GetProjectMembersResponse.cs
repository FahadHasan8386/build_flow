using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;

public class GetProjectMembersResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = string.Empty;

    public IEnumerable<ProjectMemberItem> Members { get; set; }
        = new List<ProjectMemberItem>();
}
