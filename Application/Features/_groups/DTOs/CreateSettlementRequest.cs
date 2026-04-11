using System;

namespace Application.Features._groups.DTOs
{
    public class CreateSettlementRequest
    {
        public Guid GroupId { get; set; }
        public string PayeeId { get; set; } = string.Empty; // A quién se le paga
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? ProofImageUrl { get; set; }
    }
}
