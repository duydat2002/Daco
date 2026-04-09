namespace Daco.Domain.Wallets.Aggregates
{
    public class SellerWithdrawalRequest : AggregateRoot
    {
        public Guid             SellerId          { get; private set; }
        public Guid             WalletId          { get; private set; }
        public decimal          Amount            { get; private set; }
        public decimal          Fee               { get; private set; }
        public decimal          NetAmount         { get; private set; }
        public string           BankName          { get; private set; }
        public string           BankAccountNumber { get; private set; }
        public string           BankAccountName   { get; private set; }
        public string?          BankBranch        { get; private set; }
        public WithdrawalStatus Status            { get; private set; }
        public Guid?            ApprovedBy        { get; private set; }
        public DateTime?        ApprovedAt        { get; private set; }
        public string?          RejectedReason    { get; private set; }
        public string?          TransactionCode   { get; private set; }
        public DateTime?        CompletedAt       { get; private set; }
        public string?          SellerNote        { get; private set; }
        public string?          AdminNote         { get; private set; }
        public DateTime         CreatedAt         { get; private set; }
        public DateTime?        UpdatedAt         { get; private set; }

        protected SellerWithdrawalRequest() { }
    }
}
