using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.AddProjectMember;

public class AddProjectMemberRequest
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public List<int> Roles { get; set; } = new();
}
