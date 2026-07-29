using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Common;

public class BaseModel
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; } = false;
}
