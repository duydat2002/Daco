namespace Daco.Domain.Wallets.Enums
{
    public enum SellerTransactionType
    {
        OrderPayment = 0,
        Withdrawal   = 1,
        Refund       = 2,
        Commission   = 3,
        Adjustment   = 4,
        Penalty      = 5
    }
}
