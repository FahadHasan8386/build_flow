using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.GetUsers;

public record GetUsersQuery : IRequest<GetUsersResponse>;
