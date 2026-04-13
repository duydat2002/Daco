namespace Daco.Infrastructure.Persistence.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("cart_items");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.CartId)
                .HasColumnName("cart_id")
                .IsRequired();

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.VariantId)
                .HasColumnName("variant_id")
                .IsRequired(false);

            builder.Property(a => a.Quantity)
                .HasColumnName("quantity")
                .HasDefaultValue(1)
                .IsRequired();

            builder.Property(a => a.IsSelected)
                .HasColumnName("is_selected")
                .HasDefaultValue(true);

            builder.Property(a => a.PriceSnapshot)
                .HasColumnName("price_snapshot")
                .HasColumnType("decimal(15,2)")
                .IsRequired(false);

            builder.Property(a => a.AddedAt)
                .HasColumnName("added_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Shop>()
                .WithMany()
                .HasForeignKey(a => a.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ProductVariant>()
               .WithMany()
               .HasForeignKey(a => a.VariantId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.CartId)
                .HasDatabaseName("idx_cart_items_cart");

            builder.HasIndex(a => a.ShopId)
                .HasDatabaseName("idx_cart_items_shop");

            builder.HasIndex(a => new { a.CartId, a.IsSelected })
                .HasDatabaseName("idx_cart_items_selected");

            builder.HasIndex(a => new { a.CartId, a.ProductId, a.VariantId })
                .IsUnique()
                .HasDatabaseName("uq_cart_item");
        }
    }
}
