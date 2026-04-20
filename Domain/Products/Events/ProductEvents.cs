using Daco.Domain.Shops.Aggregates;

namespace Daco.Domain.Products.Events
{
    public class ProductApprovedEvent : DomainEvent
    {
        public Guid ProductId         { get; init; }
        public Guid ShopId            { get; init; }
        public Guid ApprovedBy { get; init; }

        public ProductApprovedEvent(Guid productId, Guid shopId, Guid approvedBy)
        {
            ProductId  = productId;
            ShopId     = shopId;
            ApprovedBy = approvedBy;
        }
    }

    public class ProductSuspendedEvent : DomainEvent
    {
        public Guid   ProductId   { get; init; }
        public Guid   ShopId      { get; init; }
        public Guid   SuspendedBy { get; init; }
        public string Reason      { get; init; }

        public ProductSuspendedEvent(Guid productId, Guid shopId, Guid suspendedBy, string reason)
        {
            ProductId   = productId;
            ShopId      = shopId;
            SuspendedBy = suspendedBy;
            Reason      = reason;
        }
    }

    public class ProductUnSuspendedEvent : DomainEvent
    {
        public Guid   ProductId { get; init; }
        public Guid   ShopId      { get; init; }
        public Guid   UnSuspendedBy { get; init; }
        public string Reason { get; init; }

        public ProductUnSuspendedEvent(Guid productId, Guid shopId, Guid unSuspendedBy, string reason)
        {
            ProductId     = productId;
            ShopId        = shopId;
            UnSuspendedBy = unSuspendedBy;
            Reason        = reason;
        }
    }

    public class ProductRemovedEvent : DomainEvent
    {
        public Guid   ProductId { get; init; }
        public Guid   RemovedBy { get; init; }
        public Guid ShopId { get; init; }

        public string Reason { get; init; }

        public ProductRemovedEvent(Guid productId, Guid shopId, Guid removedBy, string reason)
        {
            ProductId = productId;
            ShopId = shopId;
            RemovedBy = removedBy; 
            Reason = reason;
        }
    }
}
