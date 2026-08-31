using BuildFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuildFlow.Application.Interfaces.Repositories;

public interface INotificationRepository
{
     Task AddAsync(Notification notification);

     Task<IEnumerable<Notification>> GetByUserAsync(Guid userId , Guid tenantId);

     Task<Notification?> GetByIdAsync(Guid nitificationId , Guid userId , Guid tenantId);

     Task MarkAsReadAsync(Guid notificationId, Guid userId, Guid tenantId);

     Task MarkAllAsReadAsync(Guid userId, Guid tenantId);

     Task DeleteAsync(Guid notificationId, Guid userId, Guid tenantId);

}
