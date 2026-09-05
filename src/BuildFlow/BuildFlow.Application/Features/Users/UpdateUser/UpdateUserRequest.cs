using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUser;

public class UpdateUserRequest
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
}
