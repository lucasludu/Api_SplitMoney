namespace Models.Request._expenses
{
    public class ExpensePaymentRequest
    {
        public string UserId { get; set; } = string.Empty;
        public decimal AmountPaid { get; set; }
    }
}
