using BuildFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities;

public class Project : BaseModel
{
    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsArchived { get; set; } = false;

    public Guid CreatedByUserId { get; set; }

}
