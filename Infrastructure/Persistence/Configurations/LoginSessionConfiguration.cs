namespace Daco.Infrastructure.Persistence.Configurations
{
    public class LoginSessionConfiguration : IEntityTypeConfiguration<LoginSession>
    {
        public void Configure(EntityTypeBuilder<LoginSession> builder)
        {
            builder.ToTable("login_sessions");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id");

            builder.Property(a => a.LoginProvider)
                .HasColumnName("login_provider")
                .HasMaxLength(50)
                .IsRequired();

            // Session info
            builder.Property(a => a.Token)
                .HasColumnName("token")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.RefreshToken)
                .HasColumnName("refresh_token")
                .HasMaxLength(255)
                .IsRequired(false);

            // User agent
            builder.Property(a => a.UserAgent)
                .HasColumnName("user_agent")
                .IsRequired(false);

            builder.Property(a => a.DeviceType)
                .HasColumnName("device_type")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.IpAddress)
                .HasColumnName("ip_address")
                .HasMaxLength(50)
                .IsRequired();

            // Status
            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.RevokedAt)
                .HasColumnName("revoked_at")
                .IsRequired(false);

            builder.Property(a => a.RevokeReason)
                .HasColumnName("revoke_reason")
                .HasMaxLength(255)
                .IsRequired(false);


            // Timestamps
            builder.Property(a => a.LoginAt)
                .HasColumnName("login_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.LastActivityAt)
                .HasColumnName("last_activity_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            // Indexes
            builder.HasIndex(s => s.Token)
                .IsUnique()
                .HasFilter("is_active = true")
                .HasDatabaseName("idx_sessions_token");

            builder.HasIndex(s => new { s.UserId, s.IsActive })
                .HasDatabaseName("idx_sessions_user");

            builder.HasIndex(s => s.ExpiresAt)
                .HasFilter("is_active = true")
                .HasDatabaseName("idx_sessions_expires");
        }
    }
}
