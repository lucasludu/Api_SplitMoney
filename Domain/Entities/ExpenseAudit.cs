using Domain.Common;
using System;

namespace Domain.Entities
{
    public class ExpenseAudit : AuditableBaseEntity
    {
        public Guid ExpenseId { get; set; }
        public Expense Expense { get; set; } = null!;
        public string ModifiedByUserId { get; set; } = string.Empty;
        public ApplicationUser ModifiedByUser { get; set; } = null!;
        public string Action { get; set; } = string.Empty; // e.g., "Updated Title", "Updated Amount", "Splits Changed"
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
    }
}
