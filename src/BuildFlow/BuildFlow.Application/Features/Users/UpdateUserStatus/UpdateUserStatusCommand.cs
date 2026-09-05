using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Users.UpdateUserStatus;

public record UpdateUserStatusCommand(Guid UserId, UpdateUserStatusRequest Request) : IRequest<UpdateUserStatusResponse>;
