using BuildFlow.Application.Features.Notifications.GetNotifications;
using BuildFlow.Application.Features.Notifications.MarkAsRead;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildFlow.api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var query = new GetNotificationsQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var command = new MarkNotificationAsReadCommand(notificationId);

            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
    }
}
