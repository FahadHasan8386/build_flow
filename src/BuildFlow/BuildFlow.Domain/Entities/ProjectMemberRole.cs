using BuildFlow.Domain.Common;
using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities;

public class ProjectMemberRole : BaseModel
{
    public Guid ProjectMemberId { get; set; }

    public ProjectMemberRoleType Role { get; set; }
}
