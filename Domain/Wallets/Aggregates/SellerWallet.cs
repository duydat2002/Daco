namespace Daco.Domain.Wallets.Aggregates
{
    public class SellerWallet : AggregateRoot
    {
        private readonly List<SellerWalletTransaction> _sellerWalletTransactions = new();

        public Guid      SellerId         { get; private set; }
        public decimal   AvailableBalance { get; private set; }
        public decimal   PendingBalance   { get; private set; }
        public decimal   FrozenBalance    { get; private set; }
        public decimal   TotalEarned      { get; private set; }
        public decimal   TotalWithdrawn   { get; private set; }
        public decimal   TotalCommission  { get; private set; }
        public decimal   TotalRefunded    { get; private set; }
        public bool      IsActive         { get; private set; }
        public DateTime  CreatedAt        { get; private set; }
        public DateTime? UpdatedAt        { get; private set; }

        public IReadOnlyCollection<SellerWalletTransaction> SellerWalletTransactions => _sellerWalletTransactions.AsReadOnly();

        protected SellerWallet() { }

        public void RefundWithdrawal(decimal amount)
        {
            Guard.AgainstNegativeOrZero(amount, nameof(amount));

            var transaction = SellerWalletTransaction.Create(
                walletId: Id,
                sellerId: SellerId,
                transactionType: SellerTransactionType.Adjustment,
                amount: amount,
                balanceBefore: AvailableBalance,
                balanceAfter: AvailableBalance + amount,
                referenceType: "withdrawal_rejected",
                description: "Withdrawal rejected - amount refunded");

            AvailableBalance += amount;
            UpdatedAt = DateTime.UtcNow;

            _sellerWalletTransactions.Add(transaction);
        }
    }
}
