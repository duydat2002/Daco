namespace Daco.Infrastructure.Persistence.Configurations
{
    public class CategoryAttributeConfiguration : IEntityTypeConfiguration<CategoryAttribute>
    {
        public void Configure(EntityTypeBuilder<CategoryAttribute> builder)
        {
            builder.ToTable("category_attributes");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.Property(a => a.AttributeName)
                .HasColumnName("attribute_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.AttributeSlug)
                .HasColumnName("attribute_slug")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.InputType)
                .HasColumnName("input_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.Property(a => a.AttributeUnitList)
                .HasColumnName("attribute_unit_list")
                .HasColumnType("VARCHAR(50)[]")
                .IsRequired(false);

            builder.Property(a => a.IsRequired)
                .HasColumnName("is_required")
                .HasDefaultValue(false);

            builder.Property(a => a.IsVariation)
                .HasColumnName("is_variation")
                .HasDefaultValue(false);

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.Unit)
                .HasColumnName("unit")
                .HasMaxLength(50)
                .IsRequired(false);

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
            builder.HasMany(a => a.CategoryAttributeValues)
                .WithOne()
                .HasForeignKey(a => a.AttributeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.CategoryAttributeValues)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_categoryAttributeValues");

            // Indexes
            builder.HasIndex(a => a.CategoryId)
                .HasDatabaseName("idx_category_attributes_category");

            builder.HasIndex(a => a.IsVariation)
                .HasDatabaseName("idx_category_attributes_variation");
        }
    }
}
