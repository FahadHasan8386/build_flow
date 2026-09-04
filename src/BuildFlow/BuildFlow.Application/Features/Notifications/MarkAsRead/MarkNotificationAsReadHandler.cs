using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using BuildFlow.Shared.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.MarkAsRead;

public class MarkNotificationAsReadHandler : IRequestHandler<MarkNotificationAsReadCommand,
     MarkNotificationAsReadResponse>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public MarkNotificationAsReadHandler(
        INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<MarkNotificationAsReadResponse> Handle(
        MarkNotificationAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var tenantId = _currentUserService.TenantId;

        var notification =
            await _notificationRepository.GetByIdAsync(
                request.NotificationId,
                userId,
                tenantId);

        if (notification is null)
        {
            return new MarkNotificationAsReadResponse
            {
                Success = false,
                Message = "Notification not found."
            };
        }

        if (notification.IsRead)
        {
            return new MarkNotificationAsReadResponse
            {
                Success = true,
                Message = "Notification is already marked as read."
            };
        }

        await _notificationRepository.MarkAsReadAsync(
            request.NotificationId,
            userId,
            tenantId);

        return new MarkNotificationAsReadResponse
        {
            Success = true,
            Message = "Notification marked as read."
        };
    }
}
