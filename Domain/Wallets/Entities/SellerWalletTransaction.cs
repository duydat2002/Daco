namespace Daco.Domain.Wallets.Entities
{
    public class SellerWalletTransaction : Entity
    {
        public Guid                  WalletId { get; private set; }
        public Guid                  SellerId { get; private set; }
        public SellerTransactionType TransactionType { get; private set; }
        public decimal               Amount { get; private set; }
        public decimal               BalanceBefore { get; private set; }
        public decimal               BalanceAfter { get; private set; }
        public string?               ReferenceType { get; private set; }
        public Guid?                 ReferenceId { get; private set; }
        public string?               Description { get; private set; }
        public TransactionStatus     Status { get; private set; }
        public DateTime              CreatedAt { get; private set; }

        protected SellerWalletTransaction() { }

        public static SellerWalletTransaction Create(
            Guid walletId,
            Guid sellerId,
            SellerTransactionType transactionType,
            decimal amount,
            decimal balanceBefore,
            decimal balanceAfter,
            string? referenceType = null,
            Guid? referenceId = null,
            string? description = null,
            TransactionStatus status = TransactionStatus.Completed)
        {
            Guard.Against(walletId == Guid.Empty, "WalletId is required");
            Guard.Against(sellerId == Guid.Empty, "SellerId is required");
            Guard.AgainstNegativeOrZero(amount, nameof(amount));

            return new SellerWalletTransaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletId,
                SellerId = sellerId,
                TransactionType = transactionType,
                Amount = amount,
                BalanceBefore = balanceBefore,
                BalanceAfter = balanceAfter,
                ReferenceType = referenceType,
                ReferenceId = referenceId,
                Description = description,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
