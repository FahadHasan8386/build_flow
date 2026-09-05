using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.GetUserById;

public class GetUserByIdResponse : ApiResponse
{
    public UserDetailsDto? User { get; set; }
}
