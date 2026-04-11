namespace Application.Features._expenses.DTOs
{
    public class BalanceResponse
    {
        public string DebtorId { get; set; } = string.Empty;
        public string DebtorName { get; set; } = string.Empty;
        public string CreditorId { get; set; } = string.Empty;
        public string CreditorName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
