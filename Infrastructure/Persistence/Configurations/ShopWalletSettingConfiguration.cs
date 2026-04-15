namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShopWalletSettingConfiguration : IEntityTypeConfiguration<ShopWalletSetting>
    {
        public void Configure(EntityTypeBuilder<ShopWalletSetting> builder)
        {
            builder.ToTable("shop_wallet_settings");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.HasIndex(a => a.ShopId)
                .IsUnique();

            builder.Property(a => a.TransactionPinHash)
                .HasColumnName("transaction_pin_hash")
                .IsRequired();

            builder.Property(a => a.PinEnabled)
                .HasColumnName("pin_enabled")
                .HasDefaultValue(false);

            builder.Property(a => a.PinUpdatedAt)
                .HasColumnName("pin_updated_at")
                .IsRequired(false);

            builder.Property(a => a.AutoWithdrawalEnabled)
                .HasColumnName("auto_withdrawal_enabled")
                .HasDefaultValue(false);

            builder.Property(a => a.WithdrawalFrequency)
                .HasColumnName("withdrawal_frequency")
                .HasMaxLength(20)
                .HasDefaultValue("manual");

            builder.Property(a => a.WithdrawalDayOfMonth)
                .HasColumnName("withdrawal_day_of_month")
                .IsRequired(false);

            builder.Property(a => a.WithdrawalDaysOfMonth)
                .HasColumnName("withdrawal_days_of_month")
                .HasColumnType("int[]")
                .IsRequired(false);

            builder.Property(a => a.MinWithdrawalAmount)
                .HasColumnName("min_withdrawal_amount")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(100000);

            builder.Property(a => a.HoldPeriodDays)
                .HasColumnName("hold_period_days")
                .HasDefaultValue(7);

            builder.Property(a => a.NotifyWithdrawalSuccess)
                .HasColumnName("notify_withdrawal_success")
                .HasDefaultValue(true);

            builder.Property(a => a.NotifyLowBalance)
                .HasColumnName("notify_low_balance")
                .HasDefaultValue(true);

            builder.Property(a => a.LowBalanceThreshold)
                .HasColumnName("low_balance_threshold")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(50000);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.ShopId)
                .HasDatabaseName("idx_wallet_settings_shop");
        }
    }
}
