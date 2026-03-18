namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.ToTable("sellers");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(s => s.BusinessType)
                .HasColumnName("business_type")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(s => s.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(SellerStatus.Pending);

            builder.Property(s => s.IsVerified)
                .HasColumnName("is_verified")
                .HasDefaultValue(false);

            builder.Property(s => s.IsOfficial)
                .HasColumnName("is_official")
                .HasDefaultValue(false);

            builder.Property(s => s.VerifiedAt)
                .HasColumnName("verified_at")
                .IsRequired(false);

            // Individual KYC
            builder.Property(s => s.IdentityNumber)
                .HasColumnName("identity_number")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(s => s.IdentityFrontUrl)
                .HasColumnName("identity_front_url")
                .IsRequired(false);

            builder.Property(s => s.IdentityBackUrl)
                .HasColumnName("identity_back_url")
                .IsRequired(false);

            builder.Property(s => s.IdentityVerified)
                .HasColumnName("identity_verified")
                .HasDefaultValue(false);

            // Company KYC
            builder.Property(s => s.CompanyName)
                .HasColumnName("company_name")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(s => s.BusinessLicenseNumber)
                .HasColumnName("business_license_number")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(s => s.BusinessLicenseUrl)
                .HasColumnName("business_license_url")
                .IsRequired(false);

            builder.Property(s => s.TaxCode)
                .HasColumnName("tax_code")
                .HasMaxLength(50)
                .IsRequired(false);

            // Timestamps
            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(s => s.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(s => s.DeletedAt)
                .HasColumnName("deleted_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(s => s.UserId)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_seller_user");

            builder.HasIndex(s => s.Status)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_seller_status");
        }
    }
}
