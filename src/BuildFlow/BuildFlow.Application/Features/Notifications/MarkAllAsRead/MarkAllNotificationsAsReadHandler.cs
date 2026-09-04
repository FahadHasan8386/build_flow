using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.MarkAllAsRead;

public class MarkAllNotificationsAsReadHandler: IRequestHandler<MarkAllNotificationsAsReadCommand,
    MarkAllNotificationsAsReadResponse>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public MarkAllNotificationsAsReadHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<MarkAllNotificationsAsReadResponse> Handle(MarkAllNotificationsAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var tenantId = _currentUserService.TenantId;

        await _notificationRepository.MarkAllAsReadAsync(
            userId,
            tenantId);

        return new MarkAllNotificationsAsReadResponse
        {
            Success = true,
            Message = "All notifications marked as read."
        };
    }
}
