namespace Daco.Domain.Orders.Entities
{
    public class OrderStatusHistory : Entity
    {
        public Guid        OrderId   { get; private set; }
        public OrderStatus Status    { get; private set; }
        public string?     Note      { get; private set; }
        public Guid?       CreatedBy { get; private set; }  
        public DateTime    CreatedAt { get; private set; }

        protected OrderStatusHistory() { }

        public static OrderStatusHistory Create(
            Guid orderId,
            OrderStatus status,
            Guid? createdBy = null,
            string? note = null)
        {
            Guard.Against(orderId == Guid.Empty, "OrderId is required");

            return new OrderStatusHistory
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Status = status,
                Note = note,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
