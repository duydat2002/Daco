namespace Daco.Domain.Wallets.Enums
{
    public enum BuyerTransactionType
    {
        Topup      = 0, //Nạp tiền
        Purchase   = 1, //Thanh toán đơn hàng
        Refund     = 2, //Hoàn tiền
        Adjustment = 3, //Điều chỉnh (admin)
    }
}
