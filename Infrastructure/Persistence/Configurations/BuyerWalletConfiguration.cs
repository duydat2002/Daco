namespace Daco.Infrastructure.Persistence.Configurations
{
    public class BuyerWalletConfiguration : IEntityTypeConfiguration<BuyerWallet>
    {
        public void Configure(EntityTypeBuilder<BuyerWallet> builder)
        {
            builder.ToTable("buyer_wallets");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasIndex(a => a.UserId)
                .IsUnique();

            builder.Property(a => a.AvailableBalance)
                .HasColumnName("available_balance")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.PendingBalance)
                .HasColumnName("pending_balance")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalTopup)
                .HasColumnName("total_topup")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalSpent)
                .HasColumnName("total_spent")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalRefunded)
                .HasColumnName("total_refunded")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("update_at")
                .IsRequired(false);

            // FK
            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.BuyerWalletTransactions)
                .WithOne()
                .HasForeignKey(v => v.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.BuyerWalletTransactions)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_buyerWalletTransactions");

            // Indexes
            builder.HasIndex(a => a.UserId)
                .HasDatabaseName("idx_buyer_wallets_user");
        }
    }
}
