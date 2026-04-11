namespace Application.Features._expenses.DTOs
{
    public class ExpenseRequest
    {
        public string Title { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public Guid GroupId { get; set; }
        public string? PayerId { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Currency { get; set; } = "USD";
        public List<ExpenseSplitRequest> Splits { get; set; } = new();
        public List<ExpensePaymentRequest> Payments { get; set; } = new();
    }
}
