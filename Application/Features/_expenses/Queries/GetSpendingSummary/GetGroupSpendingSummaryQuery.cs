using MediatR;
using Application.Wrappers;
using System;

namespace Application.Features.Expenses.Queries.GetSpendingSummary
{
    public class GetGroupSpendingSummaryQuery : IRequest<Response<GroupSpendingSummaryResponse>>
    {
        public Guid GroupId { get; set; }
    }
}
