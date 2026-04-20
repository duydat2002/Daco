namespace Daco.Domain.Sellers.Events
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

    public class SellerKycSubmittedEvent : DomainEvent
    {
        public Guid SellerId { get; init; }
        public Guid UserId { get; init; }
        public string BusinessType { get; init; }

        public SellerKycSubmittedEvent(Guid sellerId, Guid userId, string businessType)
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
        public Guid ApprovedBy { get; init; }

        public SellerApprovedEvent(Guid sellerId, Guid userId, Guid approvedBy)
        {
            SellerId = sellerId;
            UserId = userId;
            ApprovedBy = approvedBy;
        }
    }

    public class SellerRejectedEvent : DomainEvent
    {
        public Guid   SellerId   { get; init; }
        public Guid   UserId     { get; init; }
        public Guid   RejectedBy { get; init; }
        public string Reason   { get; init; }

        public SellerRejectedEvent(Guid sellerId, Guid userId, Guid rejectedBy, string reason)
        {
            SellerId   = sellerId;
            UserId     = userId;
            RejectedBy = userId;
            Reason     = reason;
        }
    }

    public class SellerSuspendedEvent : DomainEvent
    {
        public Guid   SellerId    { get; init; }
        public Guid   UserId      { get; init; }
        public Guid   SuspendedBy { get; init; }
        public string Reason      { get; init; }

        public SellerSuspendedEvent(Guid sellerId, Guid userId, Guid suspendedBy, string reason)
        {
            SellerId    = sellerId;
            UserId      = userId;
            SuspendedBy = suspendedBy;
            Reason      = reason;
        }
    }

    public class SellerReinstatedEvent : DomainEvent
    {
        public Guid SellerId { get; init; }
        public Guid UserId { get; init; }
        public Guid ReinstatedBy { get; init; }

        public SellerReinstatedEvent(Guid sellerId, Guid userId, Guid reinstatedBy)
        {
            SellerId = sellerId;
            UserId = userId;
            ReinstatedBy = reinstatedBy;
        }
    }
}
