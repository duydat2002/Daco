namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductDynamicAttributeConfiguration : IEntityTypeConfiguration<ProductDynamicAttribute>
    {
        public void Configure(EntityTypeBuilder<ProductDynamicAttribute> builder)
        {
            builder.ToTable("product_dynamic_attributes");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.AttributeId)
                .HasColumnName("attribute_id")
                .IsRequired();

            builder.Property(a => a.ValueText)
                .HasColumnName("value_text")
                .IsRequired(false);

            builder.Property(a => a.ValueNumber)
                .HasColumnName("value_number")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired(false);

            builder.Property(a => a.ValueDate)
                .HasColumnName("value_date")
                .IsRequired(false);

            builder.Property(a => a.ValueBoolean)
                .HasColumnName("value_boolean")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<CategoryAttribute>()
               .WithMany()
               .HasForeignKey(a => a.AttributeId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.ProductId, a.AttributeId })
                .IsUnique();

            builder.HasIndex(a => a.ProductId)
                .HasDatabaseName("idx_product_dyn_attrs_product");

            builder.HasIndex(a => a.AttributeId)
                .HasDatabaseName("idx_product_dyn_attrs_attribute");
        }
    }
}
