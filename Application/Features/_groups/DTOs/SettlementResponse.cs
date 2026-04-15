using System;

namespace Application.Features._groups.DTOs
{
    public class SettlementResponse
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public string PayerId { get; set; } = string.Empty;
        public string PayeeId { get; set; } = string.Empty;
        public string PayeeName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? ProofImageUrl { get; set; }
    }
}
