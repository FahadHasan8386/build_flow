using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.CreateUser;

public record CreateUserCommand(CreateUserRequest Request ) : IRequest<CreateUserResponse>;
