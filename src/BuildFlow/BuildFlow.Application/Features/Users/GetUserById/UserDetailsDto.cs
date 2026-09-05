using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.GetUserById
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
