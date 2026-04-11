using Application.Wrappers;
using MediatR;

namespace Application.Features.Expenses.Queries
{
    public class GetExpenseAuditQuery : IRequest<Response<ExpenseAuditResponse>>
    {
        public Guid ExpenseId { get; set; }
    }
}
