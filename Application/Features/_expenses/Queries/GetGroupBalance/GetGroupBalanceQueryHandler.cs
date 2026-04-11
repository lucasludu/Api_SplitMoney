using Application.Features.Expenses.Queries.GetGroupBalance;
using Application.Interfaces;
using Application.Specification._expenses;
using Application.Specification._settlements;
using Application.Specification._user;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Application.Features._expenses.DTOs;

namespace Application.Features.Expenses.Queries.GetGroupBalance
{
    public class GetGroupBalanceQueryHandler : IRequestHandler<GetGroupBalanceQuery, Response<List<BalanceResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBalanceEngineService _balanceEngine;

        public GetGroupBalanceQueryHandler(IBalanceEngineService balanceEngine, IUnitOfWork unitOfWork)
        {
            _balanceEngine = balanceEngine;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<List<BalanceResponse>>> Handle(GetGroupBalanceQuery request, CancellationToken cancellationToken)
        {
            // 1. Obtener gastos con sus divisiones
            var expensesSpec = new ExpensesByGroupSpecification(request.GroupId);
            var listExpenses = await _unitOfWork.RepositoryAsync<Expense>().ListAsync(expensesSpec);

            // 2. Obtener liquidaciones (pagos realizados)
            var settlementsSpec = new SettlementsByGroupSpecification(request.GroupId);
            var listSettlements = await _unitOfWork.RepositoryAsync<Settlement>().ListAsync(settlementsSpec);

            // 3. Ejecutar algoritmo de simplificación
            var simplifiedBalances = _balanceEngine.CalculateSimplifiedBalances(request.GroupId, listExpenses, listSettlements);

            // 4. Mapear a DTO incluyendo nombres de usuarios
            var userSpecification = new UsersByGroupSpecification(request.GroupId);
            var users = await _unitOfWork.RepositoryAsync<ApplicationUser>().ListAsync(userSpecification);

            var result = simplifiedBalances.Select(b =>
            {
                var debtor = users.FirstOrDefault(u => u.Id == b.DebtorId);
                var creditor = users.FirstOrDefault(u => u.Id == b.CreditorId);

                return new BalanceResponse
                {
                    DebtorId = b.DebtorId,
                    DebtorName = debtor != null 
                        ? $"{debtor.FirstName} {debtor.LastName}".Trim() 
                        : "Usuario desconocido",
                    CreditorId = b.CreditorId,
                    CreditorName = creditor != null 
                        ? $"{creditor.FirstName} {creditor.LastName}".Trim() 
                        : "Usuario desconocido",
                    Amount = b.Amount
                };
            }).ToList();

            return new Response<List<BalanceResponse>>(result, "Saldos calculados correctamente.");
        }
    }

}
