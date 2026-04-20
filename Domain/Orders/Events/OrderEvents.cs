namespace Daco.Domain.Orders.Events
{
    public class OrderPlacedEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }
        public string OrderCode { get; init; }
        public decimal TotalAmount { get; init; }
        public PaymentMethod PaymentMethod { get; init; }

        public OrderPlacedEvent(
            Guid orderId, Guid userId, Guid shopId,
            string orderCode, decimal totalAmount, PaymentMethod paymentMethod)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
            OrderCode = orderCode;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
        }
    }

    public class OrderPaidEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }
        public decimal TotalAmount { get; init; }

        public OrderPaidEvent(Guid orderId, Guid userId, Guid shopId, decimal totalAmount)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
            TotalAmount = totalAmount;
        }
    }

    public class OrderConfirmedEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid ShopId { get; init; }
        public Guid UserId { get; init; }

        public OrderConfirmedEvent(Guid orderId, Guid shopId, Guid userId)
        {
            OrderId = orderId;
            ShopId = shopId;
            UserId = userId;
        }
    }

    public class OrderShippedEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }
        public string TrackingNumber { get; init; }

        public OrderShippedEvent(Guid orderId, Guid userId, Guid shopId, string trackingNumber)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
            TrackingNumber = trackingNumber;
        }
    }

    public class OrderDeliveredEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }

        public OrderDeliveredEvent(Guid orderId, Guid userId, Guid shopId)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
        }
    }

    public class OrderCompletedEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }
        public decimal TotalAmount { get; init; }

        public OrderCompletedEvent(Guid orderId, Guid userId, Guid shopId, decimal totalAmount)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
            TotalAmount = totalAmount;
        }
    }

    public class OrderCancelledEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }
        public string CancelledBy { get; init; }
        public string Reason { get; init; }
        public bool WasPaid { get; init; }

        public OrderCancelledEvent(
            Guid orderId, Guid userId, Guid shopId,
            string cancelledBy, string reason, bool wasPaid)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
            CancelledBy = cancelledBy;
            Reason = reason;
            WasPaid = wasPaid;
        }
    }

    public class OrderRefundRequestedEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }
        public decimal Amount { get; init; }

        public OrderRefundRequestedEvent(Guid orderId, Guid userId, Guid shopId, decimal amount)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
            Amount = amount;
        }
    }

    public class OrderRefundCompletedEvent : DomainEvent
    {
        public Guid OrderId { get; init; }
        public Guid UserId { get; init; }
        public Guid ShopId { get; init; }
        public decimal Amount { get; init; }

        public OrderRefundCompletedEvent(Guid orderId, Guid userId, Guid shopId, decimal amount)
        {
            OrderId = orderId;
            UserId = userId;
            ShopId = shopId;
            Amount = amount;
        }
    }
}
