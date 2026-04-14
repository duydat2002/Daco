namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShippingServiceConfiguration : IEntityTypeConfiguration<ShippingService>
    {
        public void Configure(EntityTypeBuilder<ShippingService> builder)
        {
            builder.ToTable("shipping_services");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProviderId)
                .HasColumnName("provider_id")
                .IsRequired();

            builder.Property(a => a.ShippingTypeId)
                .HasColumnName("shipping_type_id")
                .IsRequired();

            builder.Property(a => a.ServiceName)
                .HasColumnName("service_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.MaxWeight)
                .HasColumnName("max_weight")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.MaxCodAmount)
                .HasColumnName("max_cod_amount")
                .HasColumnType("DECIMAL(10,2)")
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
            builder.HasOne<ShippingType>()
               .WithMany()
               .HasForeignKey(a => a.ShippingTypeId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.ProviderId, a.ShippingTypeId })
                .IsUnique();

            builder.HasIndex(a => a.ProviderId)
                .HasDatabaseName("idx_shipping_services_provider");

            builder.HasIndex(a => a.ShippingTypeId)
                .HasDatabaseName("idx_shipping_services_type");
        }
    }
}
