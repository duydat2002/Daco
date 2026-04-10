using Daco.Domain.Brands.Aggregates;

namespace Daco.Infrastructure.Persistence.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable("brands");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.BrandName)
                .HasColumnName("brand_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.BrandName)
                .IsUnique();

            builder.Property(a => a.BrandSlug)
                .HasColumnName("brand_slug")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(x => x.BrandSlug)
                .IsUnique();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.WebsiteUrl)
                .HasColumnName("website_url")
                .IsRequired(false);

            builder.Property(a => a.LogoUrl)
                .HasColumnName("logo_url")
                .IsRequired(false);

            builder.Property(a => a.SampleImages)
                .HasColumnName("sample_images")
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'[]'::jsonb");

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.BrandName)
              .HasFilter("is_active = TRUE")
              .HasDatabaseName("idx_brands_name");

            builder.HasIndex()
               .HasDatabaseName("idx_brands_search")
               .HasMethod("gin")
               .HasAnnotation("Npgsql:IndexExpression",
                   "to_tsvector('simple', brand_name)");
        }
    }
}
