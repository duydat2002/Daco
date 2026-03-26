namespace Daco.Application.Administration.UserManagement.DTOs
{
    public class UserTransactionListItemDTO
    {
        public Guid     Id              { get; set; }
        public string   TransactionType { get; set; } = null!; // topup, purchase, refund, adjustment
        public decimal  Amount          { get; set; }
        public decimal  BalanceBefore   { get; set; }
        public decimal  BalanceAfter    { get; set; }
        // Reference
        public string?  ReferenceType   { get; set; }  // order, topup_request
        public Guid?    ReferenceId     { get; set; }
        public string   Status          { get; set; } = null!;
        public string?  Description     { get; set; }
        public DateTime CreatedAt       { get; set; }
    }

    public class UserWalletSummaryDTO
    {
        public decimal AvailableBalance { get; set; }
        public decimal PendingBalance   { get; set; }
        public decimal TotalTopup       { get; set; }
        public decimal TotalSpent       { get; set; }
        public decimal TotalRefunded    { get; set; }
    }
}
