using Domain.Common;
using Domain.Enum;

namespace Domain.Entities
{
    public class ExpenseSplit : BaseEntity
    {
        public Guid ExpenseId { get; set; }
        public Expense Expense { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public SplitTypeEnum SplitType { get; set; }
        
        public decimal SplitValue { get; set; } 
        
        public decimal AmountOwed { get; set; } 
    }
}
