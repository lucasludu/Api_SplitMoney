using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Expenses.Queries.GetSpendingSummary
{
    public class GetGroupSpendingSummaryQueryHandler : IRequestHandler<GetGroupSpendingSummaryQuery, Response<GroupSpendingSummaryResponse>>
    {
        private readonly IApplicationDbContext _context;

        public GetGroupSpendingSummaryQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response<GroupSpendingSummaryResponse>> Handle(GetGroupSpendingSummaryQuery request, CancellationToken cancellationToken)
        {
            var expenses = await _context.Expenses
                .Include(e => e.Category)
                .Where(e => e.GroupId == request.GroupId)
                .ToListAsync(cancellationToken);

            if (!expenses.Any())
            {
                return new Response<GroupSpendingSummaryResponse>(new GroupSpendingSummaryResponse { GroupId = request.GroupId });
            }

            var totalSpending = expenses.Sum(e => e.Amount.Amount);

            var categorySummary = expenses
                .GroupBy(e => e.Category)
                .Select(g => new CategorySpendingResponse
                {
                    CategoryName = g.Key?.Name ?? "General",
                    CategoryIcon = g.Key?.IconIdentifier ?? "💰",
                    CategoryColor = g.Key?.ColorHex ?? "#000000",
                    TotalAmount = g.Sum(e => e.Amount.Amount),
                    Percentage = (double)(g.Sum(e => e.Amount.Amount) / totalSpending * 100)
                })
                .OrderByDescending(c => c.TotalAmount)
                .ToList();

            var response = new GroupSpendingSummaryResponse
            {
                GroupId = request.GroupId,
                TotalGroupSpending = totalSpending,
                Categories = categorySummary
            };

            return new Response<GroupSpendingSummaryResponse>(response);
        }
    }
}
