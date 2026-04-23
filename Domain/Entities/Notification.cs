using Domain.Common;
using Domain.Enum;

namespace Domain.Entities
{
    public class Notification : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public NotificationTypeEnum Type { get; set; }
        public Guid? RelatedId { get; set; } // e.g. ExpenseId
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
