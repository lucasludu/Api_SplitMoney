using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Application.Features._expenses.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Expenses.Queries
{
    public class GetUserDashboardQueryHandler : IRequestHandler<GetUserDashboardQuery, Response<DashboardResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBalanceEngineService _balanceEngine;

        public GetUserDashboardQueryHandler(IUnitOfWork unitOfWork, IBalanceEngineService balanceEngine)
        {
            _unitOfWork = unitOfWork;
            _balanceEngine = balanceEngine;
        }

        public async Task<Response<DashboardResponse>> Handle(GetUserDashboardQuery request, CancellationToken cancellationToken)
        {
            var dashboard = new DashboardResponse();

            // 1. Get all groups for the user
            var userGroups = await _unitOfWork.RepositoryAsync<GroupMember>()
                .Entities
                .Where(gm => gm.UserId == request.UserId)
                .Select(gm => gm.GroupId)
                .ToListAsync(cancellationToken);

            decimal totalToReceive = 0;
            decimal totalToPay = 0;

            // 2. For each group, calculate the user's net position
            foreach (var groupId in userGroups)
            {
                var groupExpenses = await _unitOfWork.RepositoryAsync<Expense>()
                    .Entities
                    .Include(e => e.Splits)
                    .Include(e => e.Payments)
                    .Where(e => e.GroupId == groupId && e.IsActive)
                    .ToListAsync(cancellationToken);

                var groupSettlements = await _unitOfWork.RepositoryAsync<Settlement>()
                    .Entities
                    .Where(s => s.GroupId == groupId && s.IsActive)
                    .ToListAsync(cancellationToken);

                var simplifiedBalances = _balanceEngine.CalculateSimplifiedBalances(groupId, groupExpenses, groupSettlements);

                // User is debtor
                totalToPay += simplifiedBalances
                    .Where(b => b.DebtorId == request.UserId)
                    .Sum(b => b.Amount);

                // User is creditor
                totalToReceive += simplifiedBalances
                    .Where(b => b.CreditorId == request.UserId)
                    .Sum(b => b.Amount);
            }

            dashboard.TotalToPay = totalToPay;
            dashboard.TotalToReceive = totalToReceive;

            // 3. Current Month Spending (Sum of user splits in this month)
            var startOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dashboard.TotalMonthSpending = await _unitOfWork.RepositoryAsync<ExpenseSplit>()
                .Entities
                .Include(es => es.Expense)
                .Where(es => es.UserId == request.UserId && es.IsActive && es.Expense.IsActive && es.Expense.Created >= startOfMonth)
                .SumAsync(es => es.AmountOwed, cancellationToken);

            // 4. Recent Expenses (limit 10)
            var recentExpenses = await _unitOfWork.RepositoryAsync<Expense>()
                .Entities
                .Include(e => e.Group)
                .Include(e => e.Category)
                .Where(e => e.IsActive && (e.Splits.Any(s => s.UserId == request.UserId) || e.Payments.Any(p => p.UserId == request.UserId)))
                .OrderByDescending(e => e.Created)
                .Take(10)
                .Select(e => new RecentExpenseResponse
                {
                    Id = e.Id,
                    Date = e.Created,
                    Description = e.Title,
                    GroupName = e.Group.Name,
                    Amount = e.Amount.Amount,
                    CategoryIcon = e.Category != null ? e.Category.IconIdentifier : "💰",
                    CategoryColor = e.Category != null ? e.Category.ColorHex : "#000000"
                })
                .ToListAsync(cancellationToken);

            dashboard.RecentExpenses = recentExpenses;

            return new Response<DashboardResponse>(dashboard);
        }
    }
}
