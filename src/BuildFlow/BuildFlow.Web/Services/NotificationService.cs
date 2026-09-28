using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BuildFlow.Web.Services
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int Type { get; set; }
        public Guid RelatedEntityId { get; set; }
        public string RelatedEntityType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetNotificationsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<NotificationDto> Notifications { get; set; } = new();
    }

    public class GetUnreadCountResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Count { get; set; }
    }

    public class MarkNotificationAsReadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class MarkAllNotificationsAsReadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public interface INotificationService
    {
        /// <summary>
        /// Get all notifications for the current user
        /// </summary>
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

    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "/api/notifications";

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NotificationDto>> GetNotificationsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetNotificationsResponse>(BaseUrl);
                return response?.Notifications ?? new List<NotificationDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting notifications: {ex.Message}");
                return new List<NotificationDto>();
            }
        }

        public async Task<int> GetUnreadCountAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<GetUnreadCountResponse>($"{BaseUrl}/unread-count");
                return response?.Count ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting unread count: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId)
        {
            try
            {
                using var content = new StringContent("");
                var response = await _httpClient.PutAsync($"{BaseUrl}/{notificationId}/read", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking notification as read: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> MarkAllAsReadAsync()
        {
            try
            {
                using var content = new StringContent("");
                var response = await _httpClient.PutAsync($"{BaseUrl}/read-all", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking all as read: {ex.Message}");
                return false;
            }
        }
    }
}
