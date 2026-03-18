namespace Daco.Domain.Shops.Events
{
    public class SellerRegisteredEvent : DomainEvent
    {
        public Guid SellerId { get; init; }
        public Guid UserId { get; init; }
        public string BusinessType { get; init; }

        public SellerRegisteredEvent(Guid sellerId, Guid userId, string businessType)
        {
            SellerId = sellerId;
            UserId = userId;
            BusinessType = businessType;
        }
    }

    public class SellerApprovedEvent : DomainEvent
    {
        public Guid SellerId { get; init; }
        public Guid UserId { get; init; }
        public Guid ApprovedByAdminId { get; init; }

        public SellerApprovedEvent(Guid sellerId, Guid userId, Guid approvedByAdminId)
        {
            SellerId = sellerId;
            UserId = userId;
            ApprovedByAdminId = approvedByAdminId;
        }
    }

    public class SellerRejectedEvent : DomainEvent
    {
        public Guid SellerId { get; init; }
        public Guid UserId { get; init; }
        public string Reason { get; init; }

        public SellerRejectedEvent(Guid sellerId, Guid userId, string reason)
        {
            SellerId = sellerId;
            UserId = userId;
            Reason = reason;
        }
    }

    public class SellerSuspendedEvent : DomainEvent
    {
        public Guid SellerId { get; init; }
        public Guid UserId { get; init; }
        public string Reason { get; init; }

        public SellerSuspendedEvent(Guid sellerId, Guid userId, string reason)
        {
            SellerId = sellerId;
            UserId = userId;
            Reason = reason;
        }
    }

    public class SellerReinstatedEvent : DomainEvent
    {
        public Guid SellerId { get; init; }
        public Guid UserId { get; init; }

        public SellerReinstatedEvent(Guid sellerId, Guid userId)
        {
            SellerId = sellerId;
            UserId = userId;
        }
    }
}
