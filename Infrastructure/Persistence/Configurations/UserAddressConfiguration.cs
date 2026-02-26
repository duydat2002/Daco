namespace Daco.Infrastructure.Persistence.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("user_addresses");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id");

            builder.Property(a => a.Label)
                .HasColumnName("label")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.AddressType)
                .HasColumnName("address_type")
                .HasMaxLength(0)
                .HasDefaultValue("home");

            // Address info
            builder.Property(a => a.RecipientName)
                .HasColumnName("recipient_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.RecipientPhone)
                .HasColumnName("recipient_phone")
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

            // Location coordinates
            builder.Property(a => a.Latitude)
                .HasColumnName("latitude")
                .HasColumnType("double precision")
                .IsRequired(false);

            builder.Property(a => a.Longitude)
                .HasColumnName("longitude")
                .HasColumnType("double precision")
                .IsRequired(false);

            builder.Property(a => a.GooglePlaceId)
                .HasColumnName("google_place_id")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.IsDefault)
                .HasColumnName("is_default")
                .HasDefaultValue(false);

            // Timestamps
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
            builder.HasIndex(a => a.UserId)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_user_addresses_user");

            builder.HasIndex(a => a.UserId)
                .HasFilter("is_default = TRUE AND deleted_at IS NULL")
                .HasDatabaseName("idx_user_default_address");
        }
    }
}
