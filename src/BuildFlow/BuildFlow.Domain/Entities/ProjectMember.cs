using BuildFlow.Domain.Common;
using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities;

public class ProjectMember : BaseModel
{
    public Guid ProjectId { get; set; }

    public Guid UserId { get; set; }

    public Guid TenantId { get; set; }

}
