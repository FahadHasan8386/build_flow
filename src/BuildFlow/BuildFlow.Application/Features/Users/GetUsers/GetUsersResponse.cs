using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.GetUsers;

public class GetUsersResponse
{
    public IEnumerable<UserListItemDto> Users { get; set; } = new List<UserListItemDto>();
}
