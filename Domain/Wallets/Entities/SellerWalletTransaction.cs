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
    }
}
