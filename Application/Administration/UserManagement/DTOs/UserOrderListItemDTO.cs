namespace Daco.Application.Administration.UserManagement.DTOs
{
    public class UserOrderListItemDTO
    {
        public Guid   Id            { get; set; }
        public string OrderCode     { get; set; } = null!;
        public string Status        { get; set; } = null!;
        public string PaymentStatus { get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;

        // Pricing
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Shop snapshot
        public Guid    ShopId   { get; set; }
        public string? ShopName { get; set; }

        // Item summary
        public int TotalItems { get; set; }
        public List<string?> ItemImages { get; set; } = new(); // ảnh thumbnail của các item (tối đa 3)

        // Timestamps
        public DateTime  CreatedAt   { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
}
