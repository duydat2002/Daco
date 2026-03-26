namespace Daco.Domain.Orders.Enums
{
    public enum OrderStatus
    {
        PendingPayment = 0,
        Pending        = 1,
        Confirmed      = 2,
        Processing     = 3,
        Shipping       = 4,
        Delivered      = 5,
        Completed      = 6,
        Cancelled      = 7,
        Refunding      = 8,
        Refunded       = 9
    }
}
