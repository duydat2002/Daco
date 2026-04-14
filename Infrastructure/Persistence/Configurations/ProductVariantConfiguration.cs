namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.ToTable("product_variants");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.Sku)
                .HasColumnName("sku")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(a => a.Sku)
                .IsUnique();

            builder.Property(a => a.Gtin)
                .HasColumnName("gtin")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.Option1Id)
                .HasColumnName("option1_id")
                .IsRequired(false);

            builder.Property(a => a.Option2Id)
                .HasColumnName("option2_id")
                .IsRequired(false);

            builder.Property(a => a.VariantName)
                .HasColumnName("variant_name")
                .HasMaxLength(5000)
                .IsRequired(false);

            builder.Property(a => a.Price)
                .HasColumnName("price")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.CompareAtPrice)
                .HasColumnName("compare_at_price")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired(false);

            builder.Property(a => a.StockQuantity)
                .HasColumnName("stock_quantity")
                .HasDefaultValue(0);

            builder.Property(a => a.ReservedQuantity)
                .HasColumnName("reserved_quantity")
                .HasDefaultValue(0);

            builder.Property(a => a.Weight)
                .HasColumnName("weight")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.Length)
                .HasColumnName("length")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.Width)
                .HasColumnName("width")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.Height)
                .HasColumnName("height")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.ImageUrl)
                .HasColumnName("image_url")
                .IsRequired(false);

            builder.Property(a => a.SoldCount)
                .HasColumnName("sold_count")
                .HasDefaultValue(0);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<ProductVariantOption>()
               .WithMany()
               .HasForeignKey(a => a.Option1Id)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<ProductVariantOption>()
               .WithMany()
               .HasForeignKey(a => a.Option2Id)
               .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => new { a.ProductId, a.Option1Id, a.Option2Id })
                .IsUnique();

            builder.HasIndex(a => a.ProductId)
                .HasDatabaseName("idx_variants_product");

            builder.HasIndex(a => a.Sku)
                .HasDatabaseName("idx_variants_sku");

            builder.HasIndex(a => new { a.ProductId, a.IsActive })
                .HasDatabaseName("idx_variants_active");
        }
    }
}
