using Domain.Entities;

namespace Domain.Services
{
    public class BalanceDomainService
    {
        private const decimal Epsilon = 0.01m; // Evitamos números mágicos y problemas de redondeo

        public List<Balance> CalculateSimplifiedBalances(Guid groupId, IEnumerable<Expense> expenses, IEnumerable<Settlement> settlements)
        {
            // 1. Calculamos cuánto ha puesto y cuánto debe cada uno (Neto)
            var netBalances = GetNetBalancesPerUser(expenses, settlements);

            // 2. Clasificamos en Deudores (pagan) y Acreedores (reciben)
            var debtors = netBalances.Where(x => x.Value < -Epsilon)
                                     .Select(x => new UserBalance(x.Key, Math.Abs(x.Value)))
                                     .OrderByDescending(x => x.Amount).ToList();

            var creditors = netBalances.Where(x => x.Value > Epsilon)
                                       .Select(x => new UserBalance(x.Key, x.Value))
                                       .OrderByDescending(x => x.Amount).ToList();

            // 3. Ejecutamos el algoritmo de simplificación
            return SimplifyDebts(groupId, debtors, creditors);
        }

        private Dictionary<string, decimal> GetNetBalancesPerUser(IEnumerable<Expense> expenses, IEnumerable<Settlement> settlements)
        {
            var balances = new Dictionary<string, decimal>();

            // Sumar lo pagado en gastos y restar lo debido
            foreach (var expense in expenses)
            {
                foreach (var p in expense.Payments ?? Enumerable.Empty<ExpensePayment>())
                    UpdateBalance(balances, p.UserId, p.AmountPaid);

                foreach (var s in expense.Splits ?? Enumerable.Empty<ExpenseSplit>())
                    UpdateBalance(balances, s.UserId, -s.AmountOwed);
            }

            // Ajustar con las liquidaciones (settlements) ya realizadas
            foreach (var s in settlements)
            {
                UpdateBalance(balances, s.PayerId, s.Amount.Amount);
                UpdateBalance(balances, s.PayeeId, -s.Amount.Amount);
            }

            return balances;
        }

        private void UpdateBalance(Dictionary<string, decimal> dict, string userId, decimal amount)
        {
            if (string.IsNullOrEmpty(userId)) return;
            dict[userId] = dict.GetValueOrDefault(userId) + amount;
        }

        private List<Balance> SimplifyDebts(Guid groupId, List<UserBalance> debtors, List<UserBalance> creditors)
        {
            var results = new List<Balance>();
            int d = 0, c = 0;

            while (d < debtors.Count && c < creditors.Count)
            {
                var debtor = debtors[d];
                var creditor = creditors[c];
                var amountToTransfer = Math.Min(debtor.Amount, creditor.Amount);

                if (amountToTransfer > Epsilon)
                {
                    results.Add(new Balance
                    {
                        Id = Guid.NewGuid(),
                        GroupId = groupId,
                        DebtorId = debtor.UserId,
                        CreditorId = creditor.UserId,
                        Amount = amountToTransfer
                    });
                }

                debtor.Amount -= amountToTransfer;
                creditor.Amount -= amountToTransfer;

                if (debtor.Amount < Epsilon) d++;
                if (creditor.Amount < Epsilon) c++;
            }

            return results;
        }

        private class UserBalance
        {
            public string UserId { get; }
            public decimal Amount { get; set; }
            public UserBalance(string userId, decimal amount) { UserId = userId; Amount = amount; }
        }
    }
}