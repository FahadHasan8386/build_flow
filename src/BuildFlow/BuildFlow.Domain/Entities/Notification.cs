using BuildFlow.Domain.Common;
using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities
{
    public class Notification : BaseModel
    {

        public Guid TenantId { get; set; }

        public Guid UserId { get; set; }

        public NotificationType Type { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public Guid? RelatedEntityId { get; set; }

        public string? RelatedEntityType { get; set; }

        public bool IsRead { get; set; }

    }
}
