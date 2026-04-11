using Application.Features._expenses.DTOs;

namespace Application.Features._expenses.DTOs
{
    public class UpdateExpenseRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime Date { get; set; }
        public Guid? CategoryId { get; set; }
        public List<ExpenseSplitRequest> Splits { get; set; } = new List<ExpenseSplitRequest>();
        public List<ExpensePaymentRequest>? Payments { get; set; } = new List<ExpensePaymentRequest>();
    }
}
