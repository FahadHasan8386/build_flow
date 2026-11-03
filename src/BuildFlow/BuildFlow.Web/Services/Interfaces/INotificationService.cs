using BuildFlow.Web.Models;

namespace BuildFlow.Web.Services.Auth
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotificationsAsync();

        Task<int> GetUnreadCountAsync();

        Task<bool> MarkAsReadAsync(Guid notificationId);

        Task<bool> MarkAllAsReadAsync();
    }
}
