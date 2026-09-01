using BuildFlow.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Interfaces.Services;

public interface INotificationService
{
    Task CreateAsync(
        Guid tenantId,
        Guid userId,
        NotificationType type,
        string title,
        string message,
        Guid? relatedEntityId = null,
        string? relatedEntityType = null,
        Guid? createdBy = null);
}