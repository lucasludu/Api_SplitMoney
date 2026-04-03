namespace Models.Request._expenses
{
    public class ExpenseRequest
    {
        public Guid GroupId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime Date { get; set; }
        public string PayerId { get; set; } = string.Empty;
        public List<ExpenseSplitRequest> Splits { get; set; } = new List<ExpenseSplitRequest>();
    }
}
