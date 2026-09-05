using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.CreateUser;

public class CreateUserResponse : ApiResponse
{
    public Guid UserId { get; set; }
}
