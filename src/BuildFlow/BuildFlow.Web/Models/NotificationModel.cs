namespace BuildFlow.Web.Models
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
}
