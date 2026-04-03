using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Balance : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public string DebtorId { get; set; } = string.Empty;
        public ApplicationUser Debtor { get; set; } = null!;

        public string CreditorId { get; set; } = string.Empty;
        public ApplicationUser Creditor { get; set; } = null!;

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
    }
}
