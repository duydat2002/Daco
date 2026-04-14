namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductVideoConfiguration : IEntityTypeConfiguration<ProductVideo>
    {
        public void Configure(EntityTypeBuilder<ProductVideo> builder)
        {
            builder.ToTable("product_videos");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.VideoUrl)
                .HasColumnName("video_url")
                .IsRequired();

            builder.Property(a => a.ThumbnailUrl)
                .HasColumnName("thumbnail_url")
                .IsRequired(false);

            builder.Property(a => a.Duration)
                .HasColumnName("duration")
                .IsRequired(false);

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // Indexes
            builder.HasIndex(a => a.ProductId)
                .HasDatabaseName("idx_product_videos_product");
        }
    }
}
