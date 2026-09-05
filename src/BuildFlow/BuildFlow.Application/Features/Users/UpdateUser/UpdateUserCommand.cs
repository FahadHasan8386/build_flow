using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUser;

public record UpdateUserCommand(Guid UserId,UpdateUserRequest Request) : IRequest<UpdateUserResponse>;
