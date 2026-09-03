using BuildFlow.Domain.Entities;
using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.GetNotifications;

public class GetNotificationsResponse : ApiResponse
{
    public IEnumerable<Notification> Notifications { get; set; }  = new List<Notification>();
}
