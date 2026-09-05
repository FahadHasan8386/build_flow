using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUserStatus;

public class UpdateUserStatusRequest
{
    public bool IsActive { get; set; }
}
