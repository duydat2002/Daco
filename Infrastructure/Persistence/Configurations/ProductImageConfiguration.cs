namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_images");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.ImageUrl)
                .HasColumnName("image_url")
                .IsRequired();

            builder.Property(a => a.AltText)
                .HasColumnName("alt_text")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.IsCover)
                .HasColumnName("is_cover")
                .HasDefaultValue(false);

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // Indexes
            builder.HasIndex(a => a.ProductId)
                .HasFilter("is_cover = TRUE")
                .HasDatabaseName("idx_product_cover_image")
                .IsUnique();

            builder.HasIndex(a => a.ProductId)
                .HasDatabaseName("idx_product_images_product");
        }
    }
}
