namespace Daco.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(o => o.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(o => o.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(o => o.OrderCode)
                .HasColumnName("order_code")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(o => o.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(OrderStatus.PendingPayment);

            builder.Property(o => o.PaymentStatus)
                .HasColumnName("payment_status")
                .HasConversion<int>()
                .HasDefaultValue(PaymentStatus.Unpaid);

            builder.Property(o => o.PaymentMethod)
                .HasColumnName("payment_method")
                .HasConversion<int>()
                .IsRequired();

            // Pricing
            builder.Property(o => o.Subtotal)
                .HasColumnName("subtotal")
                .HasColumnType("decimal(15,2)")
                .IsRequired();

            builder.Property(o => o.ShippingFee)
                .HasColumnName("shipping_fee")
                .HasColumnType("decimal(15,2)")
                .HasDefaultValue(0);

            builder.Property(o => o.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasColumnType("decimal(15,2)")
                .HasDefaultValue(0);

            builder.Property(o => o.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(15,2)")
                .IsRequired();

            // Shipping address
            builder.Property(o => o.ShippingAddress)
                .HasColumnName("shipping_address")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(o => o.RecipientName)
                .HasColumnName("recipient_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(o => o.RecipientPhone)
                .HasColumnName("recipient_phone")
                .HasMaxLength(20)
                .IsRequired();

            // Notes
            builder.Property(o => o.BuyerNote)
                .HasColumnName("buyer_note")
                .IsRequired(false);

            builder.Property(o => o.SellerNote)
                .HasColumnName("seller_note")
                .IsRequired(false);

            // Cancellation
            builder.Property(o => o.CancelledBy)
                .HasColumnName("cancelled_by")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(o => o.CancelReason)
                .HasColumnName("cancel_reason")
                .IsRequired(false);

            builder.Property(o => o.CancelledAt)
                .HasColumnName("cancelled_at")
                .IsRequired(false);

            // Timestamps
            builder.Property(o => o.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(o => o.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(o => o.ConfirmedAt)
                .HasColumnName("confirmed_at")
                .IsRequired(false);

            builder.Property(o => o.ShippedAt)
                .HasColumnName("shipped_at")
                .IsRequired(false);

            builder.Property(o => o.DeliveredAt)
                .HasColumnName("delivered_at")
                .IsRequired(false);

            builder.Property(o => o.CompletedAt)
                .HasColumnName("completed_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(o => new { o.UserId, o.CreatedAt })
                .HasDatabaseName("idx_orders_user");

            builder.HasIndex(o => new { o.ShopId, o.CreatedAt })
                .HasDatabaseName("idx_orders_shop");

            builder.HasIndex(o => o.OrderCode)
                .IsUnique()
                .HasDatabaseName("idx_orders_code");

            builder.HasIndex(o => new { o.Status, o.CreatedAt })
                .HasDatabaseName("idx_orders_status");

            // Relations
            builder.HasMany(o => o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(o => o.OrderItems)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_orderItems");

            builder.HasMany(o => o.Payments)
                .WithOne()
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(o => o.Payments)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_payments");
        }
    }
}
