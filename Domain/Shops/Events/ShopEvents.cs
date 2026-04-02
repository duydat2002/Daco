namespace Daco.Domain.Shops.Events
{
    public class ShopCreatedEvent : DomainEvent
    {
        public Guid ShopId { get; init; }
        public Guid SellerId { get; init; }

        public ShopCreatedEvent(Guid shopId, Guid sellerId)
        {
            ShopId = shopId;
            SellerId = sellerId;
        }
    }

    public class ShopSuspendedEvent : DomainEvent
    {
        public Guid ShopId { get; init; }
        public Guid SellerId { get; init; }
        public string Reason { get; init; }

        public ShopSuspendedEvent(Guid shopId, Guid sellerId, string reason)
        {
            ShopId = shopId;
            SellerId = sellerId;
            Reason = reason;
        }
    }

    public class ShopReinstatedEvent : DomainEvent
    {
        public Guid ShopId { get; init; }
        public Guid SellerId { get; init; }

        public ShopReinstatedEvent(Guid shopId, Guid sellerId)
        {
            ShopId = shopId;
            SellerId = sellerId;
        }
    }

    public class ShopClosedEvent : DomainEvent
    {
        public Guid ShopId { get; init; }
        public Guid SellerId { get; init; }
        public string Reason { get; init; }

        public ShopClosedEvent(Guid shopId, Guid sellerId, string reason)
        {
            ShopId = shopId;
            SellerId = sellerId;
            Reason = reason;
        }
    }
}
