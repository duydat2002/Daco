namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerShippingConfigConfiguration : IEntityTypeConfiguration<SellerShippingConfig>
    {
        public void Configure(EntityTypeBuilder<SellerShippingConfig> builder)
        {
            builder.ToTable("seller_shipping_configs");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
                .IsRequired();

            builder.HasIndex(a => a.SellerId)
                .IsUnique();

            builder.Property(a => a.EnableExpress)
                .HasColumnName("enable_express")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableSameDay)
                .HasColumnName("enable_same_day")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableFast)
                .HasColumnName("enable_fast")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableEconomy)
                .HasColumnName("enable_economy")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableBulky)
                .HasColumnName("enable_bulky")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableExpressCod)
                .HasColumnName("enable_express_cod")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableSameDayCod)
                .HasColumnName("enable_same_day_cod")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableFastCod)
                .HasColumnName("enable_fast_cod")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableEconomyCod)
                .HasColumnName("enable_economy_cod")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableBulkyCod)
                .HasColumnName("enable_bulky_cod")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Seller>()
               .WithMany()
               .HasForeignKey(a => a.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.SellerId)
                .HasDatabaseName("idx_seller_shipping_seller");
        }
    }
}
