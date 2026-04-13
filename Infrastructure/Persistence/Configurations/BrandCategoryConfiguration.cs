namespace Daco.Infrastructure.Persistence.Configurations
{
    public class BrandCategoryConfiguration : IEntityTypeConfiguration<BrandCategory>
    {
        public void Configure(EntityTypeBuilder<BrandCategory> builder)
        {
            builder.ToTable("brand_categories");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.BrandId)
                .HasColumnName("brand_id")
                .IsRequired();

            builder.Property(a => a.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(a => new { a.BrandId, a.CategoryId })
                .IsUnique()
                .HasDatabaseName("uq_brand_category");

            // FK
            builder.HasOne<Brand>()
                .WithMany()
                .HasForeignKey(a => a.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.BrandId)
               .HasDatabaseName("idx_brand_categories_brand");

            builder.HasIndex(a => a.CategoryId)
              .HasDatabaseName("idx_brand_categories_category");
        }
    }
}
