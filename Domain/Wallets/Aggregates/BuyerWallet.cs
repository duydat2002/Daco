namespace Daco.Domain.Wallets.Aggregates
{
    public class BuyerWallet : AggregateRoot
    {
        private readonly List<BuyerWalletTransaction> _buyerWalletTransactions = new();

        public Guid      UserId           { get; private set; }
        public decimal   AvailableBalance { get; private set; }
        public decimal   PendingBalance   { get; private set; }
        public decimal   TotalTopup       { get; private set; }
        public decimal   TotalSpent       { get; private set; }
        public decimal   TotalRefunded    { get; private set; }
        public bool      IsActive         { get; private set; }
        public DateTime  CreatedAt        { get; private set; }
        public DateTime? UpdatedAt        { get; private set; }

        public IReadOnlyCollection<BuyerWalletTransaction> BuyerWalletTransactions => _buyerWalletTransactions.AsReadOnly();

        protected BuyerWallet() { }
    }
}
