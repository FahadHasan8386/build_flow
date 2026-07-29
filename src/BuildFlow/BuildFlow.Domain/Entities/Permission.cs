using BuildFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities;

public class Permission : BaseModel
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; }
}
