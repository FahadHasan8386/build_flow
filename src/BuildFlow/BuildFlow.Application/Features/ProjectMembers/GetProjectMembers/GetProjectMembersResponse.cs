using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;

public class GetProjectMembersResponse : ApiResponse
{
    public IEnumerable<ProjectMemberItem> Members { get; set; }
        = new List<ProjectMemberItem>();
}
