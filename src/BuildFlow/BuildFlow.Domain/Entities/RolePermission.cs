using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities;

public class RolePermission
{
    public Guid RoleId { get; set; }

    public Guid PermissionId { get; set; }
}
