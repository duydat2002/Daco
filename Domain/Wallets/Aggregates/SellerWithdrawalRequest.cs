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

        public void Approve(Guid approvedBy, string? adminNote = null)
        {
            Guard.Against(Status != WithdrawalStatus.Pending, "Only pending withdrawals can be approved");

            Status = WithdrawalStatus.Approved;
            ApprovedBy = approvedBy;
            ApprovedAt = DateTime.UtcNow;
            AdminNote = adminNote;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject(string reason, string? adminNote = null)
        {
            Guard.Against(
                Status != WithdrawalStatus.Pending && Status != WithdrawalStatus.Approved,
                "Cannot reject a withdrawal at this status");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            Status = WithdrawalStatus.Rejected;
            RejectedReason = reason;
            AdminNote = adminNote;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Complete(string transactionCode, string? adminNote = null)
        {
            Guard.Against(
                Status != WithdrawalStatus.Approved && Status != WithdrawalStatus.Processing,
                "Only approved or processing withdrawals can be completed");
            Guard.AgainstNullOrEmpty(transactionCode, nameof(transactionCode));

            Status = WithdrawalStatus.Completed;
            TransactionCode = transactionCode;
            AdminNote = adminNote;
            CompletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
