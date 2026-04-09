using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specification._expenses
{
    public class ExpensesByGroupSpecification : Specification<Expense>
    {
        public ExpensesByGroupSpecification(Guid groupId)
        {
            Query.Where(e => e.GroupId == groupId && e.IsActive)
                 .Include(e => e.Splits)
                 .Include(e => e.Payments);
        }
    }
}
