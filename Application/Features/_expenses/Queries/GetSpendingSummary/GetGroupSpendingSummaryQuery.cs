using Application.Wrappers;
using MediatR;

namespace Application.Features.Expenses.Queries.GetSpendingSummary
{
    public class GetGroupSpendingSummaryQuery : IRequest<Response<GroupSpendingSummaryResponse>>
    {
        public Guid GroupId { get; set; }
    }
}
