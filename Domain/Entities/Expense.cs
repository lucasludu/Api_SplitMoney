using Domain.Common;

namespace Domain.Entities
{
    public class Expense : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public string Title { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; } 
        public string Currency { get; set; } = "USD";
        public decimal? ExchangeRateToGroupCurrency { get; set; } 

        public DateTime Date { get; set; }
        public string? ReceiptUrl { get; set; } 

        public string PayerId { get; set; } = string.Empty;
        public ApplicationUser Payer { get; set; } = null!;

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<ExpenseSplit> Splits { get; set; } = new List<ExpenseSplit>();
    }
}
