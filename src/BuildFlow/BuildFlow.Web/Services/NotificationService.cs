using BuildFlow.Web.Models;
using BuildFlow.Web.Services.Auth;
using System.Net.Http.Json;

namespace BuildFlow.Web.Services
{
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
