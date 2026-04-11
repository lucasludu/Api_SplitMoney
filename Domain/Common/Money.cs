namespace Domain.Common
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { Currency = string.Empty; } // For EF Core

        public Money(decimal amount, string currency)
        {
            if (amount < 0) throw new ArgumentException("Amount cannot be negative", nameof(amount));
            if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency cannot be empty", nameof(currency));

            Amount = amount;
            Currency = currency;
        }

        protected IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType()) return false;
            var other = (Money)obj;
            return Amount == other.Amount && Currency == other.Currency;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public static bool operator ==(Money left, Money right) => Equals(left, right);
        public static bool operator !=(Money left, Money right) => !Equals(left, right);
        
        public override string ToString() => $"{Amount:N2} {Currency}";
    }
}
