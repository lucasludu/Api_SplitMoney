using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._user
{
    public class UsersByGroupSpecification : Specification<ApplicationUser>
    {
        public UsersByGroupSpecification(Guid groupId)
        {
            Query
                .Where(u => u.GroupMemberships.Any(gm => gm.GroupId == groupId));
        }
    }
}
