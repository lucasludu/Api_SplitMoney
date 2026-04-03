using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class BalanceEngineService : IBalanceEngineService
    {
        public List<Balance> CalculateSimplifiedBalances(Guid groupId, List<Expense> expenses, List<Settlement> settlements)
        {
            var netBalances = new Dictionary<string, decimal>();

            // 1. Calculate net debt for each user
            foreach (var expense in expenses)
            {
                // Payer gets back the total minus their own share
                if (!netBalances.ContainsKey(expense.PayerId)) netBalances[expense.PayerId] = 0;
                netBalances[expense.PayerId] += expense.TotalAmount;

                foreach (var split in expense.Splits)
                {
                    if (!netBalances.ContainsKey(split.UserId)) netBalances[split.UserId] = 0;
                    netBalances[split.UserId] -= split.AmountOwed;
                }
            }

            // 2. Adjust for settlements (repayments)
            foreach (var settlement in settlements)
            {
                if (!netBalances.ContainsKey(settlement.PayerId)) netBalances[settlement.PayerId] = 0;
                netBalances[settlement.PayerId] += settlement.Amount;

                if (!netBalances.ContainsKey(settlement.PayeeId)) netBalances[settlement.PayeeId] = 0;
                netBalances[settlement.PayeeId] -= settlement.Amount;
            }

            // 3. Separate debtors and creditors
            var debtors = netBalances.Where(x => x.Value < -0.01m)
                                     .Select(x => new UserBalance { UserId = x.Key, Amount = Math.Abs(x.Value) })
                                     .OrderByDescending(x => x.Amount).ToList();
            
            var creditors = netBalances.Where(x => x.Value > 0.01m)
                                      .Select(x => new UserBalance { UserId = x.Key, Amount = x.Value })
                                      .OrderByDescending(x => x.Amount).ToList();

            var simplifiedBalances = new List<Balance>();

            // 4. Simplify debts (Greedy approach)
            int d = 0, c = 0;
            while (d < debtors.Count && c < creditors.Count)
            {
                var debtor = debtors[d];
                var creditor = creditors[c];
                var amount = Math.Min(debtor.Amount, creditor.Amount);

                if (amount > 0.01m)
                {
                    simplifiedBalances.Add(new Balance
                    {
                        GroupId = groupId,
                        DebtorId = debtor.UserId,
                        CreditorId = creditor.UserId,
                        Amount = amount
                    });
                }

                debtor.Amount -= amount;
                creditor.Amount -= amount;

                if (debtor.Amount < 0.01m) d++;
                if (creditor.Amount < 0.01m) c++;
            }

            return simplifiedBalances;
        }

        private class UserBalance
        {
            public string UserId { get; set; } = string.Empty;
            public decimal Amount { get; set; }
        }
    }
}
