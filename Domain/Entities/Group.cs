using Domain.Common;

namespace Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string DefaultCurrency { get; set; } = "USD";

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public ICollection<Balance> Balances { get; set; } = new List<Balance>();
        public ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();
    }
}
