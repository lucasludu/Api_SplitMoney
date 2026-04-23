using Domain.Enum;

namespace Application.Features._notifications.DTOs
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public NotificationTypeEnum Type { get; set; }
        public Guid? RelatedId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
