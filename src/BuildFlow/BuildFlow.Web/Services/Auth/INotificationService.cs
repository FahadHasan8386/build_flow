using BuildFlow.Web.Models;

namespace BuildFlow.Web.Services.Auth
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotificationsAsync();

        /// <summary>
        /// Get count of unread notifications
        /// </summary>
        Task<int> GetUnreadCountAsync();

        /// <summary>
        /// Mark a single notification as read
        /// </summary>
        Task<bool> MarkAsReadAsync(Guid notificationId);

        /// <summary>
        /// Mark all unread notifications as read
        /// </summary>
        Task<bool> MarkAllAsReadAsync();
    }
}
