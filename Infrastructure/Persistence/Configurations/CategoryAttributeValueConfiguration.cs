namespace Daco.Infrastructure.Persistence.Configurations
{
    public class CategoryAttributeValueConfiguration : IEntityTypeConfiguration<CategoryAttributeValue>
    {
        public void Configure(EntityTypeBuilder<CategoryAttributeValue> builder)
        {
            builder.ToTable("category_attribute_values");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.AttributeId)
                .HasColumnName("attribute_id")
                .IsRequired();

            builder.Property(a => a.Value)
                .HasColumnName("value")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.IsActive)
                .HasColumnName("true")
                .HasDefaultValue(255);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValue("NOW()");

            // Indexes
            builder.HasIndex(a => a.AttributeId)
                .HasDatabaseName("idx_attr_values_attribute");
        }
    }
}
