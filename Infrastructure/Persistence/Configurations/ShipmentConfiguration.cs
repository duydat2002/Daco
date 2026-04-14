namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.ToTable("shipments");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.ServiceId)
                .HasColumnName("service_id")
                .IsRequired();

            builder.Property(a => a.ProviderId)
                .HasColumnName("provider_id")
                .IsRequired();

            builder.Property(a => a.TrackingNumber)
                .HasColumnName("tracking_number")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(a => a.TrackingNumber)
                .IsUnique();

            builder.Property(a => a.Weight)
                .HasColumnName("weight")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired();

            builder.Property(a => a.ShippingFee)
                .HasColumnName("shipping_fee")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.CodFee)
                .HasColumnName("cod_fee")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalFee)
                .HasColumnName("total_fee")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.CodAmount)
                .HasColumnName("cod_amount")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(ShipmentStatus.Pending);

            builder.Property(a => a.EstimatedDeliveryAt)
                .HasColumnName("estimated_delivery_at")
                .IsRequired(false);

            builder.Property(a => a.DeliveredAt)
                .HasColumnName("delivered_at")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Shop>()
               .WithMany()
               .HasForeignKey(a => a.ShopId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ShippingService>()
               .WithMany()
               .HasForeignKey(a => a.ServiceId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<ShippingProvider>()
               .WithMany()
               .HasForeignKey(a => a.ProviderId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ShipmentTrackings)
                .WithOne()
                .HasForeignKey(v => v.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ShipmentTrackings)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_shipmentTrackings");

            // Indexes
            builder.HasIndex(a => a.OrderId)
                .HasDatabaseName("idx_shipments_order");

            builder.HasIndex(a => new { a.OrderId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_shipments_order");

            builder.HasIndex(a => a.TrackingNumber)
                .HasDatabaseName("idx_shipments_tracking");

            builder.HasIndex(a => a.Status)
                .HasDatabaseName("idx_shipments_status");
        }
    }
}
