using BuildFlow.Domain.Common;
using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities;

public class Tenant : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public TenantStatus Status { get; set; } = TenantStatus.Active;

}
