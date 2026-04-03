using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string DefaultCurrency { get; set; } = "USD";

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<GroupMember> GroupMemberships { get; set; } = new List<GroupMember>();
        public ICollection<Expense> PaidExpenses { get; set; } = new List<Expense>();
        public ICollection<ExpenseSplit> OwedSplits { get; set; } = new List<ExpenseSplit>();
        public ICollection<Category> CustomCategories { get; set; } = new List<Category>();
    }
}
