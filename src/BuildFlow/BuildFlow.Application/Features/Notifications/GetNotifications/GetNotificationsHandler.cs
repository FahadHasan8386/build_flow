using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.GetNotifications;

public class GetNotificationsHandler : IRequestHandler<GetNotificationsQuery, GetNotificationsResponse>
{
    private readonly INotificationRepository _notifiicationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetNotificationsHandler(INotificationRepository notifiicationRepository,
        ICurrentUserService currentUserService)
    {
        _notifiicationRepository = notifiicationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetNotificationsResponse> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var tenantId = _currentUserService.TenantId;

        var notifications = await _notifiicationRepository.GetByUserAsync(userId, tenantId);


        return new GetNotificationsResponse
        {
            Success = true,
            Message = "Notifications retrieved successfully.",
            Notifications = notifications
        };
    }

}
