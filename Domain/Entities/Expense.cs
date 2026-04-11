using Domain.Common;

namespace Domain.Entities
{
    public class Expense : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public Money Amount { get; set; } = null!;
        public decimal? ExchangeRateToGroupCurrency { get; set; } 

        public DateTime Date { get; set; }
        public string? ReceiptUrl { get; set; } 

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<ExpenseSplit> Splits { get; set; } = new List<ExpenseSplit>();
        public ICollection<ExpensePayment> Payments { get; set; } = new List<ExpensePayment>();
    }
}
