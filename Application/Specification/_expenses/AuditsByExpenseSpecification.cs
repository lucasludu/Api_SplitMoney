using Ardalis.Specification;
using Domain.Entities;
using System;

namespace Application.Specification._expenses
{
    public class AuditsByExpenseSpecification : Specification<ExpenseAudit>
    {
        public AuditsByExpenseSpecification(Guid expenseId)
        {
            Query.Where(a => a.ExpenseId == expenseId)
                 .Include(a => a.ModifiedByUser)
                 .OrderByDescending(a => a.ChangeDate);
        }
    }
}
