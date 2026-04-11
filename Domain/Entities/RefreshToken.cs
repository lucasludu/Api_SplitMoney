using Domain.Common;

namespace Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; }
        public new bool IsActive => Revoked == null && !IsExpired;

        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
