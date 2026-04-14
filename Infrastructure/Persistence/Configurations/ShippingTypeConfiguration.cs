namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShippingTypeConfiguration : IEntityTypeConfiguration<ShippingType>
    {
        public void Configure(EntityTypeBuilder<ShippingType> builder)
        {
            builder.ToTable("shipping_types");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(a => a.Code)
                .IsUnique();

            builder.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.EstimatedDaysMin)
                .HasColumnName("estimated_days_min")
                .IsRequired(false);

            builder.Property(a => a.EstimatedDaysMax)
                .HasColumnName("estimated_days_max")
                .IsRequired(false);

            builder.Property(a => a.IconUrl)
                .HasColumnName("icon_url")
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

            builder.Property(a => a.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired(false);
        }
    }
}
