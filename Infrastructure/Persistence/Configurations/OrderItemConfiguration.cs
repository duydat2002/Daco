namespace Daco.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");

            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(oi => oi.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(oi => oi.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(oi => oi.VariantId)
                .HasColumnName("variant_id")
                .IsRequired(false);

            // Snapshot
            builder.Property(oi => oi.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(oi => oi.VariantName)
                .HasColumnName("variant_name")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(oi => oi.ProductImage)
                .HasColumnName("product_image")
                .IsRequired(false);

            builder.Property(oi => oi.Sku)
                .HasColumnName("sku")
                .HasMaxLength(100)
                .IsRequired(false);

            // Pricing
            builder.Property(oi => oi.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(15,2)")
                .IsRequired();

            builder.Property(oi => oi.Quantity)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(oi => oi.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasColumnType("decimal(15,2)")
                .HasDefaultValue(0);

            builder.Property(oi => oi.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(15,2)")
                .IsRequired();

            builder.Property(oi => oi.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // Indexes
            builder.HasIndex(oi => oi.OrderId)
                .HasDatabaseName("idx_order_items_order");

            builder.HasIndex(oi => oi.ProductId)
                .HasDatabaseName("idx_order_items_product");
        }
    }
}
