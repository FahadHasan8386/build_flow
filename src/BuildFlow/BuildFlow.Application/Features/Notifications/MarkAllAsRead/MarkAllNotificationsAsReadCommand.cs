using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.MarkAllAsRead;

public record MarkAllNotificationsAsReadCommand: IRequest<MarkAllNotificationsAsReadResponse>;
