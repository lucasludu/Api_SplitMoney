using Application.Features._groups.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Groups.Queries
{
    public class GetGroupSpendingBreakdownQueryHandler : IRequestHandler<GetGroupSpendingBreakdownQuery, Response<GroupSpendingBreakdownResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBalanceEngineService _balanceEngine;

        public GetGroupSpendingBreakdownQueryHandler(IUnitOfWork unitOfWork, IBalanceEngineService balanceEngine)
        {
            _unitOfWork = unitOfWork;
            _balanceEngine = balanceEngine;
        }

        public async Task<Response<GroupSpendingBreakdownResponse>> Handle(GetGroupSpendingBreakdownQuery request, CancellationToken cancellationToken)
        {
            var group = await _unitOfWork.RepositoryAsync<Group>()
                .Entities
                .Include(g => g.Members)
                    .ThenInclude(gm => gm.User)
                .FirstOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);

            if (group == null)
                return new Response<GroupSpendingBreakdownResponse>("Grupo no encontrado.");

            var expenses = await _unitOfWork.RepositoryAsync<Expense>()
                .Entities
                .Include(e => e.Splits)
                .Include(e => e.Payments)
                .Where(e => e.GroupId == request.GroupId && e.IsActive)
                .ToListAsync(cancellationToken);

            var settlements = await _unitOfWork.RepositoryAsync<Settlement>()
                .Entities
                .Where(s => s.GroupId == request.GroupId && s.IsActive)
                .ToListAsync(cancellationToken);

            var totalSpending = expenses.Sum(e => e.Amount.Amount);

            // Calcular balances simplificados para obtener la posición neta de cada uno
            var simplifiedBalances = _balanceEngine.CalculateSimplifiedBalances(request.GroupId, expenses, settlements);

            var breakdown = new GroupSpendingBreakdownResponse
            {
                GroupId = group.Id.ToString(),
                GroupName = group.Name,
                TotalGroupExpense = totalSpending,
                Members = group.Members.Select(gm =>
                {
                    var userId = gm.UserId;
                    
                    // Conteo de transacciones en las que participa (pagó o se le dividió)
                    var count = expenses.Count(e => e.Payments.Any(p => p.UserId == userId) || e.Splits.Any(s => s.UserId == userId));

                    // Balance Neto: (Lo que otros le deben) - (Lo que él debe)
                    var owedToHim = simplifiedBalances.Where(b => b.CreditorId == userId).Sum(b => b.Amount);
                    var heOwes = simplifiedBalances.Where(b => b.DebtorId == userId).Sum(b => b.Amount);
                    
                    return new MemberSpendingResponse
                    {
                        UserId = userId,
                        FullName = gm.User != null ? $"{gm.User.FirstName} {gm.User.LastName}".Trim() : "Usuario desconocido",
                        TransactionCount = count,
                        NetBalance = owedToHim - heOwes
                    };
                }).OrderByDescending(m => m.NetBalance).ToList()
            };

            return new Response<GroupSpendingBreakdownResponse>(breakdown);
        }
    }
}
