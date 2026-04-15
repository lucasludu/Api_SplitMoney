using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            var totalSpending = expenses.Sum(e => e.Amount?.Amount ?? 0);

            var categorySummary = expenses
                .Where(e => e.Amount != null && e.Amount.Amount > 0)
                .GroupBy(e => new { 
                    Name = e.Category?.Name ?? "General",
                    Icon = e.Category?.IconIdentifier ?? "💰",
                    Color = e.Category?.ColorHex ?? "#000000"
                })
                .Select(g => new CategorySpendingResponse
                {
                    CategoryName = g.Key.Name,
                    CategoryIcon = g.Key.Icon,
                    CategoryColor = g.Key.Color,
                    TotalAmount = g.Sum(e => e.Amount!.Amount),
                    Percentage = totalSpending > 0 
                        ? (double)(g.Sum(e => e.Amount!.Amount) / totalSpending * 100m) 
                        : 0
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
