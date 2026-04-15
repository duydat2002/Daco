namespace Daco.Infrastructure.Persistence.Configurations
{
    public class BuyerTopupRequestConfiguration : IEntityTypeConfiguration<BuyerTopupRequest>
    {
        public void Configure(EntityTypeBuilder<BuyerTopupRequest> builder)
        {
            builder.ToTable("buyer_topup_requests");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.WalletId)
                .HasColumnName("wallet_id")
                .IsRequired();

            builder.Property(a => a.TopupCode)
                .HasColumnName("topup_code")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(a => a.TopupCode)
                .IsUnique();

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

            builder.Property(a => a.Method)
                .HasColumnName("method")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.PaymentGateway)
                .HasColumnName("payment_gateway")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.GatewayTransactionId)
                .HasColumnName("gateway_transaction_id")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.GatewayOrderId)
                .HasColumnName("gateway_order_id")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.GatewayResponse)
                .HasColumnName("gateway_response")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.GatewayCallbackData)
                .HasColumnName("gateway_callback_data")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.BankName)
                .HasColumnName("bank_name")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.BankAccountNumber)
                .HasColumnName("bank_account_number")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.TransferReference)
                .HasColumnName("transfer_reference")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.TransferProofUrl)
                .HasColumnName("transfer_proof_url")
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(TopupStatus.Pending);

            builder.Property(a => a.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(a => a.CompletedAt)
                .HasColumnName("completed_at")
                .IsRequired(false);

            builder.Property(a => a.FailedAt)
                .HasColumnName("failed_at")
                .IsRequired(false);

            builder.Property(a => a.FailureReason)
                .HasColumnName("failure_reason")
                .IsRequired(false);

            builder.Property(a => a.UserNote)
                .HasColumnName("user_note")
                .IsRequired(false);

            builder.Property(a => a.AdminNote)
                .HasColumnName("admin_note")
                .IsRequired(false);

            // FK
            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<BuyerWallet>()
               .WithMany()
               .HasForeignKey(a => a.WalletId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_topup_user");

            builder.HasIndex(a => a.TopupCode)
                .HasDatabaseName("idx_topup_code");

            builder.HasIndex(a => new { a.Status, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_topup_status");

            builder.HasIndex(a => a.GatewayTransactionId)
                .HasDatabaseName("idx_topup_gateway_txn");

            builder.HasIndex(a => new { a.UserId, a.ExpiresAt })
                .HasFilter("status = 0")
                .HasDatabaseName("idx_topup_pending");
        }
    }
}
