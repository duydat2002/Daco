namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShopAddressConfiguration : IEntityTypeConfiguration<ShopAddress>
    {
        public void Configure(EntityTypeBuilder<ShopAddress> builder)
        {
            builder.ToTable("shop_addresses");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.Label)
                .HasColumnName("label")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.AddressType)
                .HasColumnName("address_type")
                .HasConversion<int>()
                .HasDefaultValue(ShopAddressType.Pickup);

            builder.Property(a => a.ContactName)
                .HasColumnName("contact_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.ContactPhone)
                .HasColumnName("contact_phone")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.City)
                .HasColumnName("city")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.District)
                .HasColumnName("district")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Ward)
                .HasColumnName("ward")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.AddressDetail)
                .HasColumnName("address_detail")
                .IsRequired();

            builder.Property(a => a.Latitude)
                .HasColumnName("latitude")
                .HasColumnType("DOUBLE PRECISION")
                .IsRequired();

            builder.Property(a => a.Longitude)
                .HasColumnName("longitude")
                .HasColumnType("DOUBLE PRECISION")
                .IsRequired();

            builder.Property(a => a.GooglePlaceId)
                .HasColumnName("google_place_id")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.OperatingHours)
                .HasColumnName("operating_hours")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.IsDefault)
                .HasColumnName("is_default")
                .HasDefaultValue(false);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(a => a.DeletedAt)
                .HasColumnName("deleted_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => new { a.ShopId, a.AddressType })
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_shop_addresses_shop");

            builder.HasIndex(a => new { a.ShopId, a.AddressType })
                .IsUnique()
                .HasFilter("is_default = TRUE AND address_type = 0 AND deleted_at IS NULL")
                .HasDatabaseName("idx_shop_address_default_warehouse");

            builder.HasIndex(a => new { a.ShopId, a.AddressType })
                .IsUnique()
                .HasFilter("is_default = TRUE AND address_type = 1 AND deleted_at IS NULL")
                .HasDatabaseName("idx_shop_addresses_shop");

            builder.HasIndex(a => new { a.ShopId, a.AddressType })
                .IsUnique()
                .HasFilter("is_default = TRUE AND address_type = 2 AND deleted_at IS NULL")
                .HasDatabaseName("idx_shop_addresses_shop");
        }
    }
}
