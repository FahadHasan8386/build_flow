using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.GetNotifications;

public record GetNotificationsQuery : IRequest<GetNotificationsResponse>;
