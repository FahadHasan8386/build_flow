using BuildFlow.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Domain.Entities
{
    public class UserRole : BaseModel
    {
        public Guid UserId { get; set; }

        public Guid RoleId { get; set; }
    }
}
