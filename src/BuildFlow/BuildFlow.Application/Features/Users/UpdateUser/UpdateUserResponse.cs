using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUser
{
    public class UpdateUserResponse : ApiResponse
    {
        public Guid UserId { get; set; }
    }
}
