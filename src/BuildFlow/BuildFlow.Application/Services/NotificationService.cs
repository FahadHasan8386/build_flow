using BuildFlow.Application.Interfaces.Repositories;
using BuildFlow.Application.Interfaces.Services;
using BuildFlow.Domain.Entities;
using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(
        INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task CreateAsync(
        Guid tenantId,
        Guid userId,
        NotificationType type,
        string title,
        string message,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null,
        Guid? createdBy = null)
    {
        var notification = new Notification
        {
            Id = Guid.NewGuid(),

            TenantId = tenantId,

            UserId = userId,

            Type = type,

            Title = title,

            Message = message,

            RelatedEntityId = relatedEntityId,

            RelatedEntityType = relatedEntityType,

            IsRead = false,

            CreatedAt = DateTime.UtcNow,

            CreatedBy = createdBy?.ToString(),

            IsDeleted = false
        };

        await _notificationRepository.AddAsync(notification);
    }
}
