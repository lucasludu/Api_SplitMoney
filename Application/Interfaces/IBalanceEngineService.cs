using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IBalanceEngineService
    {
        List<Balance> CalculateSimplifiedBalances(Guid groupId, List<Expense> expenses, List<Settlement> settlements);
    }
}
