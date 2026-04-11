using Application.Interfaces;
using Domain.Entities;
using Domain.Services;

namespace Application.Services
{
    public class BalanceEngineService : IBalanceEngineService
    {
        private readonly BalanceDomainService _balanceDomainService;

        public BalanceEngineService()
        {
            _balanceDomainService = new BalanceDomainService();
        }

        public List<Balance> CalculateSimplifiedBalances(Guid groupId, List<Expense> expenses, List<Settlement> settlements)
        {
            return _balanceDomainService.CalculateSimplifiedBalances(groupId, expenses, settlements);
        }
    }
}

