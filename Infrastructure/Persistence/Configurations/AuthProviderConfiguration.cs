namespace Daco.Infrastructure.Persistence.Configurations
{
    public class AuthProviderConfiguration : IEntityTypeConfiguration<AuthProvider>
    {
        public void Configure(EntityTypeBuilder<AuthProvider> builder)
        {
            builder.ToTable("auth_providers");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id");

            // Provider Info
            builder.Property(a => a.ProviderType)
                .HasColumnName("provider_type")
                .HasColumnType("provider_types")
                .IsRequired(); 

            builder.Property(a => a.ProviderKey)
                .HasColumnName("provider_key")
                .IsRequired();

            // Password
            builder.Property(a => a.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired(false);

            builder.Property(a => a.PasswordUpdatedAt)
                .HasColumnName("password_updated_at")
                .IsRequired(false);

            // Social Data
            builder.Property(a => a.ProviderUserId)
                .HasColumnName("provider_user_id")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.ProviderEmail)
                .HasColumnName("provider_email")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.ProviderName)
                .HasColumnName("provider_name")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.ProviderAvatar)
                .HasColumnName("provider_avatar")
                .IsRequired(false);

            // Tokens
            builder.Property(a => a.AccessToken)
                .HasColumnName("access_token")
                .IsRequired(false);

            builder.Property(a => a.RefreshToken)
                .HasColumnName("refresh_token")
                .IsRequired(false);

            builder.Property(a => a.TokenExpiresAt)
                .HasColumnName("token_expires_at")
                .IsRequired(false);

            // Verification
            builder.Property(a => a.IsVerified)
                .HasColumnName("is_verified")
                .HasDefaultValue(false);

            builder.Property(a => a.VerifiedAt)
                .HasColumnName("verified_at")
                .IsRequired(false);

            // Timestamps
            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.UserId)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_auth_user");

            builder.HasIndex(a => new { a.ProviderType, a.ProviderKey })
                .IsUnique()
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_auth_provider_key");

            builder.HasIndex(a => a.IsVerified)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_auth_verified");
        }
    }
}
