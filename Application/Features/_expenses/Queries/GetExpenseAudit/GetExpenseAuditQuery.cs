using Application.Wrappers;
using MediatR;

namespace Application.Features._expenses.Queries.GetExpenseAudit
{
    public class GetExpenseAuditQuery : IRequest<Response<ExpenseAuditResponse>>
    {
        public Guid ExpenseId { get; set; }
    }
}
