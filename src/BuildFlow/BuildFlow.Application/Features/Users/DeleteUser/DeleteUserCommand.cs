using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.DeleteUser;

public record DeleteUserCommand( Guid UserId) : IRequest<DeleteUserResponse>;
