using Domain.Common;

namespace Domain.Entities
{
    public class GroupMember : BaseEntity
    {
        public Guid GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;

        public DateTime JoinedAt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
