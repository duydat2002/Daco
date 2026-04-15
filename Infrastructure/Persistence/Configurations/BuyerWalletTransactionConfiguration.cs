namespace Daco.Infrastructure.Persistence.Configurations
{
    public class BuyerWalletTransactionConfiguration : IEntityTypeConfiguration<BuyerWalletTransaction>
    {
        public void Configure(EntityTypeBuilder<BuyerWalletTransaction> builder)
        {
            builder.ToTable("buyer_wallet_transactions");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.WalletId)
                .HasColumnName("wallet_id")
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.TransactionType)
                .HasColumnName("transaction_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.Amount)
                .HasColumnName("amount")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.BalanceBefore)
                .HasColumnName("balance_before")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.BalanceAfter)
                .HasColumnName("balance_after")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.ReferenceType)
                .HasColumnName("reference_type")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.ReferenceId)
                .HasColumnName("reference_id")
                .IsRequired(false);

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(TransactionStatus.Pending);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.WalletId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("buyer_wallet_transactions");

            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_buyer_trans_user");

            builder.HasIndex(a => a.TransactionType)
                .HasDatabaseName("idx_buyer_trans_type");

            builder.HasIndex(a => a.ReferenceType)
                .HasDatabaseName("idx_buyer_trans_reference_type");
        }
    }
}
