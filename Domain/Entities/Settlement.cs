using Domain.Common;

namespace Domain.Entities
{
    public class Settlement : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public string PayerId { get; set; } = string.Empty;
        public ApplicationUser Payer { get; set; } = null!;

        public string PayeeId { get; set; } = string.Empty;
        public ApplicationUser Payee { get; set; } = null!;

        public Money Amount { get; set; } = null!;
        public DateTime Date { get; set; }
        
        public string? ProofImageUrl { get; set; }
    }
}
