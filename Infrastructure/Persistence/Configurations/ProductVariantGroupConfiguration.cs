namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductVariantGroupConfiguration : IEntityTypeConfiguration<ProductVariantGroup>
    {
        public void Configure(EntityTypeBuilder<ProductVariantGroup> builder)
        {
            builder.ToTable("product_variant_groups");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.GroupName)
                .HasColumnName("group_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.GroupType)
                .HasColumnName("group_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasMany(p => p.ProductVariantOptions)
                .WithOne()
                .HasForeignKey(v => v.VariantGroupId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ProductVariantOptions)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_productVariantOptions");

            // Indexes
            builder.HasIndex(a => new { a.ProductId, a.GroupType })
                .IsUnique();

            builder.HasIndex(a => a.ProductId)
                .HasDatabaseName("idx_variant_groups_product");
        }
    }
}
