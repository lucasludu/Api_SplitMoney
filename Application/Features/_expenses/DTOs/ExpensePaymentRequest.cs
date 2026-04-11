namespace Application.Features._expenses.DTOs
{
    public class ExpensePaymentRequest
    {
        public string UserId { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
    }
}
