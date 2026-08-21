using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.ProjectMembers.GetProjectMembers;

public class ProjectMemberItem
{
    public Guid ProjectMemberId { get; set; }

    public Guid UserId { get; set; }

    public List<ProjectMemberRoleType> Roles { get; set; }
        = new();
}
