using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<GetUserByIdResponse>;
