using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._settlements
{
    public class SettlementsByGroupSpecification : Specification<Settlement>
    {
        public SettlementsByGroupSpecification(Guid groupId)
        {
            Query.Where(x => x.GroupId == groupId);
        }
    }
}
