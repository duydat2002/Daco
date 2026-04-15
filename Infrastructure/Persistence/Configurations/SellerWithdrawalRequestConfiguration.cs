namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerWithdrawalRequestConfiguration : IEntityTypeConfiguration<SellerWithdrawalRequest>
    {
        public void Configure(EntityTypeBuilder<SellerWithdrawalRequest> builder)
        {
            builder.ToTable("seller_withdrawal_requests");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
                .IsRequired();

            builder.Property(a => a.WalletId)
                .HasColumnName("wallet_id")
                .IsRequired();

            builder.Property(a => a.Amount)
                .HasColumnName("amount")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.Fee)
                .HasColumnName("fee")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.NetAmount)
                .HasColumnName("net_amount")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.BankName)
                .HasColumnName("bank_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.BankAccountNumber)
                .HasColumnName("bank_account_number")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.BankAccountName)
                .HasColumnName("bank_account_name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(a => a.BankBranch)
                .HasColumnName("bank_branch")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(WithdrawalStatus.Pending);

            builder.Property(a => a.ApprovedBy)
                .HasColumnName("approved_by")
                .IsRequired(false);

            builder.Property(a => a.ApprovedAt)
                .HasColumnName("approved_at")
                .IsRequired(false);

            builder.Property(a => a.RejectedReason)
                .HasColumnName("rejected_reason")
                .IsRequired(false);

            builder.Property(a => a.TransactionCode)
                .HasColumnName("transaction_code")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.CompletedAt)
                .HasColumnName("completed_at")
                .IsRequired(false);

            builder.Property(a => a.SellerNote)
                .HasColumnName("seller_note")
                .IsRequired(false);

            builder.Property(a => a.AdminNote)
                .HasColumnName("admin_note")
                .IsRequired(false);

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

            builder.HasOne<SellerWallet>()
               .WithMany()
               .HasForeignKey(a => a.WalletId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.SellerId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_withdrawals_seller");

            builder.HasIndex(a => new { a.Status, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_withdrawals_status");
        }
    }
}
