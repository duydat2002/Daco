namespace Daco.Infrastructure.Persistence.Configurations
{
    public class VerificationTokenConfiguration : IEntityTypeConfiguration<VerificationToken>
    {
        public void Configure(EntityTypeBuilder<VerificationToken> builder)
        {
            builder.ToTable("verification_tokens");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id");

            // Token
            builder.Property(a => a.Token)
                .HasColumnName("token")
                .IsRequired();

            builder.Property(a => a.TokenType)
                .HasColumnName("token_type")
                .HasMaxLength(50)
                .IsRequired();

            // Status
            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasDefaultValue(VerificationStatus.Pending)
                .IsRequired();

            builder.Property(a => a.Attempts)
                .HasColumnName("attempts")
                .HasDefaultValue(0);

            builder.Property(a => a.MaxAttempts)
                .HasColumnName("max_attempts")
                .HasDefaultValue(5);

            // Timestamps
            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            builder.Property(a => a.VerifiedAt)
                .HasColumnName("verified_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.UserId)
                .HasDatabaseName("idx_verification_user");

            builder.HasIndex(a => new { a.Status, a.ExpiresAt })
                .HasDatabaseName("idx_verification_status");
        }
    }
}
