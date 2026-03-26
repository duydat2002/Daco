namespace Daco.Domain.Orders.ValueObjects
{
    public sealed class Money : ValueObject
    {
        public decimal Amount { get; }

        private Money(decimal amount)
        {
            Amount = amount;
        }

        public static Money Zero => new(0m);

        public static Money Of(decimal amount)
        {
            Guard.AgainstNegative(amount, nameof(amount));
            return new Money(amount);
        }

        public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
        public static Money operator -(Money a, Money b)
        {
            var result = a.Amount - b.Amount;
            Guard.Against(result < 0, "Result cannot be negative");
            return new Money(result);
        }
        public static bool operator >(Money a, Money b) => a.Amount > b.Amount;
        public static bool operator <(Money a, Money b) => a.Amount < b.Amount;
        public static bool operator >=(Money a, Money b) => a.Amount >= b.Amount;
        public static bool operator <=(Money a, Money b) => a.Amount <= b.Amount;

        public static implicit operator decimal(Money m) => m.Amount;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }

        public override string ToString() => Amount.ToString("N0") + " VND";
    }
}
