namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductVariantOptionConfiguration : IEntityTypeConfiguration<ProductVariantOption>
    {
        public void Configure(EntityTypeBuilder<ProductVariantOption> builder)
        {
            builder.ToTable("product_variant_options");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.VariantGroupId)
                .HasColumnName("variant_group_id")
                .IsRequired();

            builder.Property(a => a.OptionName)
                .HasColumnName("option_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.OptionValue)
                .HasColumnName("option_value")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.ImageUrl)
                .HasColumnName("image_url")
                .IsRequired(false);

            builder.Property(a => a.ColorHex)
                .HasColumnName("color_hex")
                .HasMaxLength(7)
                .IsRequired(false);

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // Indexes
            builder.HasIndex(a => a.VariantGroupId)
                .HasDatabaseName("idx_variant_options_group");
        }
    }
}
