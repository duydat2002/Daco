namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShippingProviderConfiguration : IEntityTypeConfiguration<ShippingProvider>
    {
        public void Configure(EntityTypeBuilder<ShippingProvider> builder)
        {
            builder.ToTable("shipping_providers");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProviderCode)
                .HasColumnName("provider_code")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(a => a.ProviderCode)
                .IsUnique();

            builder.Property(a => a.ProviderName)
                .HasColumnName("provider_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.LogoUrl)
                .HasColumnName("logo_url")
                .IsRequired(false);

            builder.Property(a => a.SupportCod)
                .HasColumnName("support_cod")
                .HasDefaultValue(true);

            builder.Property(a => a.SupportTracking)
                .HasColumnName("support_tracking")
                .HasDefaultValue(true);

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
            builder.HasMany(p => p.ShippingServices)
                .WithOne()
                .HasForeignKey(v => v.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ShippingServices)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_shippingServices");
        }
    }
}
