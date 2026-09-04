using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.MarkAsRead;

public record MarkNotificationAsReadCommand(Guid NotificationId) : IRequest<MarkNotificationAsReadResponse>;
