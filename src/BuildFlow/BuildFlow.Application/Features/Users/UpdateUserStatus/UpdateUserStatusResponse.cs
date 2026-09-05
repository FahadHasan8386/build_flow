using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUserStatus
{
    public class UpdateUserStatusResponse : ApiResponse
    {
        public Guid UserId { get; set; }

        public bool IsActive { get; set; }
    }
}
