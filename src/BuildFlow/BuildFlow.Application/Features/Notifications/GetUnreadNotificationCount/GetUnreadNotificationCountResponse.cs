using BuildFlow.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.GetUnreadNotificationCount
{
    public class GetUnreadNotificationCountResponse : ApiResponse
    {
        public int Count { get; set; }
    }
}
