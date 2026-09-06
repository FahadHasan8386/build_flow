using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.DeleteUser;

public class DeleteUserResponse : ApiResponse
{
    public Guid UserId { get; set; }
}
