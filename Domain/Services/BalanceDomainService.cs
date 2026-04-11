using Domain.Entities;

namespace Domain.Services
{
    public class BalanceDomainService
    {
        public List<Balance> CalculateSimplifiedBalances(Guid groupId, IEnumerable<Expense> expenses, IEnumerable<Settlement> settlements)
        {
            // Calculate total paid per user
            var totalPaidByUsers = expenses
                .SelectMany(e => e.Payments ?? new List<ExpensePayment>())
                .GroupBy(p => p.UserId)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.AmountPaid));

            // Calculate total owed per user
            var totalOwedByUsers = expenses
                .SelectMany(e => e.Splits ?? new List<ExpenseSplit>())
                .GroupBy(s => s.UserId)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.AmountOwed));

            // Adjustments from settlements
            var totalSettledPayment = settlements
                .GroupBy(s => s.PayerId)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.Amount.Amount));

            var totalSettledReceipt = settlements
                .GroupBy(s => s.PayeeId)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.Amount.Amount));

            // Get all unique users
            var allUserIds = totalPaidByUsers.Keys
                .Union(totalOwedByUsers.Keys)
                .Union(totalSettledPayment.Keys)
                .Union(totalSettledReceipt.Keys)
                .Distinct();

            // Net balance = (Paid + SettledAsPayer) - (Owed + SettledAsPayee)
            var netBalances = allUserIds.ToDictionary(
                userId => userId,
                userId => (totalPaidByUsers.GetValueOrDefault(userId) + totalSettledPayment.GetValueOrDefault(userId)) - 
                          (totalOwedByUsers.GetValueOrDefault(userId) + totalSettledReceipt.GetValueOrDefault(userId))
            );

            // Separate debtors (negative balance) and creditors (positive balance)
            var debtors = netBalances.Where(x => x.Value < -0.01m)
                                     .Select(x => new UserBalance { UserId = x.Key, Amount = Math.Abs(x.Value) })
                                     .OrderByDescending(x => x.Amount).ToList();

            var creditors = netBalances.Where(x => x.Value > 0.01m)
                                       .Select(x => new UserBalance { UserId = x.Key, Amount = x.Value })
                                       .OrderByDescending(x => x.Amount).ToList();

            var simplifiedBalances = new List<Balance>();

            // Simplify debts (Greedy approach)
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
                        Id = Guid.NewGuid(),
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
