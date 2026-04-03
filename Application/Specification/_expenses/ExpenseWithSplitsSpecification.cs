using Ardalis.Specification;
using Domain.Entities;
using System;

namespace Application.Specification._expenses
{
    public class ExpenseWithSplitsSpecification : Specification<Expense>, ISingleResultSpecification<Expense>
    {
        public ExpenseWithSplitsSpecification(Guid id)
        {
            Query.Where(e => e.Id == id)
                 .Include(e => e.Splits);
        }
    }
}
