using Domain.Common;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string IconIdentifier { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#000000";

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
