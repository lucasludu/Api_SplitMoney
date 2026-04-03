using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._user
{
    public class ActiveGroupsByUserCountSpecification : Specification<GroupMember>
    {
        public ActiveGroupsByUserCountSpecification(string userId)
        {
            Query
                .Where(gm => gm.UserId == userId && gm.IsAdmin);
        }
    }
}
