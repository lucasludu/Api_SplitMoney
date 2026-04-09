using Domain.Common;

namespace Domain.Entities
{
    public class ExpensePayment : BaseEntity
    {
        public Guid ExpenseId { get; set; }
        public Expense Expense { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public decimal AmountPaid { get; set; }
    }
}
