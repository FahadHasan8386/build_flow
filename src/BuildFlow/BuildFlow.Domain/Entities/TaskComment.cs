using BuildFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities
{
    public class TaskComment : BaseModel
    {
        public Guid TenantId { get; set; }

        public Guid TaskId { get; set; }

        public Guid UserId { get; set; }

        public string Comment { get; set; } = string.Empty;
    }
}
