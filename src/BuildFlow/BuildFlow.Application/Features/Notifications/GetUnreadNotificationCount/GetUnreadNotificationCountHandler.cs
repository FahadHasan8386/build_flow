using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Security;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Features.Notifications.GetUnreadNotificationCount;

public class GetUnreadNotificationCountHandler : IRequestHandler<GetUnreadNotificationCountQuery,
    GetUnreadNotificationCountResponse>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetUnreadNotificationCountHandler(INotificationRepository notificationRepository,
        ICurrentUserService currentUserService)
    {
        _notificationRepository = notificationRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GetUnreadNotificationCountResponse> Handle(GetUnreadNotificationCountQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var tenantId = _currentUserService.TenantId;

        var count = await _notificationRepository
            .GetUnreadCountAsync(userId, tenantId);

        return new GetUnreadNotificationCountResponse
        {
            Success = true,
            Message = "Unread notification count retrieved successfully.",
            Count = count
        };
    }
}
