namespace Daco.Domain.Orders.Entities
{
    public class OrderItem : Entity
    {
        public Guid     OrderId        { get; private set; }
        public Guid     ProductId      { get; private set; }
        public Guid?    VariantId      { get; private set; }
        // Snapshot
        public string   ProductName    { get; private set; }
        public string?  VariantName    { get; private set; }
        public string?  ProductImage   { get; private set; }
        public string?  Sku            { get; private set; }
        // Pricing
        public decimal  Price          { get; private set; }
        public int      Quantity       { get; private set; }
        public decimal  DiscountAmount { get; private set; }
        public decimal  TotalAmount    { get; private set; }
        public DateTime CreatedAt      { get; private set; }

        protected OrderItem() { }

        public static OrderItem Create(
            Guid orderId,
            Guid productId,
            Guid? variantId,
            string productName,
            string? variantName,
            string? productImage,
            string? sku,
            decimal price,
            int quantity,
            decimal discountAmount = 0m)
        {
            Guard.Against(orderId == Guid.Empty, "OrderId is required");
            Guard.Against(productId == Guid.Empty, "ProductId is required");
            Guard.AgainstNullOrEmpty(productName, nameof(productName));
            Guard.Against(price <= 0, "Price must be greater than zero");
            Guard.Against(quantity <= 0, "Quantity must be greater than zero");
            Guard.AgainstNegative(discountAmount, nameof(discountAmount));

            var total = price * quantity - discountAmount;
            Guard.Against(total < 0, "Total amount cannot be negative");

            return new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = orderId,
                ProductId = productId,
                VariantId = variantId,
                ProductName = productName,
                VariantName = variantName,
                ProductImage = productImage,
                Sku = sku,
                Price = price,
                Quantity = quantity,
                DiscountAmount = discountAmount,
                TotalAmount = total,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
