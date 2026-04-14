namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShipmentTrackingConfiguration : IEntityTypeConfiguration<ShipmentTracking>
    {
        public void Configure(EntityTypeBuilder<ShipmentTracking> builder)
        {
            builder.ToTable("shipment_tracking");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShipmentId)
                .HasColumnName("shipment_id")
                .IsRequired();

            builder.Property(a => a.EventType)
                .HasColumnName("event_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.EventCode)
                .HasColumnName("event_code")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.Location)
                .HasColumnName("location")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.Address)
                .HasColumnName("address")
                .IsRequired(false);

            builder.Property(a => a.Latitude)
                .HasColumnName("latitude")
                .HasColumnType("DOUBLE PRECISION")
                .IsRequired(false);

            builder.Property(a => a.Longitude)
                .HasColumnName("longitude")
                .HasColumnType("DOUBLE PRECISION")
                .IsRequired(false);

            builder.Property(a => a.EventTime)
                .HasColumnName("event_time")
                .IsRequired();

            builder.Property(a => a.HandledBy)
                .HasColumnName("handled_by")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.ContactPhone)
                .HasColumnName("contact_phone")
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(a => a.SignatureUrl)
                .HasColumnName("signature_url")
                .IsRequired(false);

            builder.Property(a => a.PhotoUrl)
                .HasColumnName("photo_url")
                .IsRequired(false);

            builder.Property(a => a.Notes)
                .HasColumnName("notes")
                .IsRequired(false);

            builder.Property(a => a.ProviderData)
                .HasColumnName("provider_data")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // Indexes
            builder.HasIndex(a => new { a.ShipmentId, a.EventTime })
                .IsDescending(false, true)
                .HasDatabaseName("idx_shipment_tracking_shipment");

            builder.HasIndex(a => new { a.EventType, a.EventTime })
               .IsDescending(false, true)
               .HasDatabaseName("idx_shipment_tracking_event");

            builder.HasIndex(a => a.EventTime )
               .IsDescending(true)
               .HasDatabaseName("idx_shipment_tracking_time");
        }
    }
}
