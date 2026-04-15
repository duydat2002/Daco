namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerWalletConfiguration : IEntityTypeConfiguration<SellerWallet>
    {
        public void Configure(EntityTypeBuilder<SellerWallet> builder)
        {
            builder.ToTable("seller_wallets");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
                .IsRequired();

            builder.HasIndex(a => a.SellerId)
                .IsUnique();

            builder.Property(a => a.AvailableBalance)
                .HasColumnName("available_balance")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.PendingBalance)
                .HasColumnName("pending_balance")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.FrozenBalance)
                .HasColumnName("frozen_balance")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalEarned)
                .HasColumnName("total_earned")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalWithdrawn)
                .HasColumnName("total_withdrawn")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalCommission)
                .HasColumnName("total_commission")
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
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Seller>()
               .WithMany()
               .HasForeignKey(a => a.SellerId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.SellerWalletTransactions)
                .WithOne()
                .HasForeignKey(v => v.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.SellerWalletTransactions)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_sellerWalletTransactions");

            // Indexes
            builder.HasIndex(a => a.SellerId)
                .HasDatabaseName("idx_seller_wallets_shop");
        }
    }
}
