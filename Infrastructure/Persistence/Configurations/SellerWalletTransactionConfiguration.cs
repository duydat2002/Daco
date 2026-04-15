namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerWalletTransactionConfiguration : IEntityTypeConfiguration<SellerWalletTransaction>
    {
        public void Configure(EntityTypeBuilder<SellerWalletTransaction> builder)
        {
            builder.ToTable("seller_wallet_transactions");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.WalletId)
                .HasColumnName("wallet_id")
                .IsRequired();

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
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
            builder.HasOne<Seller>()
               .WithMany()
               .HasForeignKey(a => a.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.WalletId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_seller_trans_wallet");

            builder.HasIndex(a => new { a.SellerId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_seller_trans_seller");

            builder.HasIndex(a => a.TransactionType)
                .HasDatabaseName("idx_seller_trans_type");
        }
    }
}
