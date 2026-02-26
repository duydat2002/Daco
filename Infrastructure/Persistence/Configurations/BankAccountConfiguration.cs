namespace Daco.Infrastructure.Persistence.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("user_bank_accounts");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id");

            // Bank info
            builder.Property(a => a.BankCode)
                .HasColumnName("bank_code")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.BankName)
                .HasColumnName("bank_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.AccountNumber)
                .HasColumnName("account_number")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.AccountHolder)
                .HasColumnName("account_holder")
                .HasMaxLength(100)
                .IsRequired();

            // Status
            builder.Property(a => a.IsDefault)
                .HasColumnName("is_default")
                .HasDefaultValue(false);

            builder.Property(a => a.IsVerified)
                .HasColumnName("is_verified")
                .HasDefaultValue(false);

            // Timestamps
            builder.Property(a => a.VerifiedAt)
                .HasColumnName("verified_at")
                .IsRequired(false);

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
                .HasDatabaseName("idx_user_bank_accounts");
        }
    }
}
