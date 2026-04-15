namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShopNotificationSettingConfiguration : IEntityTypeConfiguration<ShopNotificationSetting>
    {
        public void Configure(EntityTypeBuilder<ShopNotificationSetting> builder)
        {
            builder.ToTable("shop_notification_settings");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.HasIndex(a => a.ShopId)
                .IsUnique();

            builder.Property(a => a.NotifyNewOrder)
                .HasColumnName("notify_new_order")
                .HasDefaultValue(true);

            builder.Property(a => a.NotifyOrderPaid)
                .HasColumnName("notify_order_paid")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyOrderCancelled)
                .HasColumnName("notify_order_cancelled")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyOrderReturned)
                .HasColumnName("notify_order_returned")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyProductLowStock)
                .HasColumnName("notify_product_low_stock")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyProductOutOfStock)
                .HasColumnName("notify_product_out_of_stock")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyProductReview)
                .HasColumnName("notify_product_review")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyProductQuestion)
                .HasColumnName("notify_product_question")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyNewFollower)
                .HasColumnName("notify_new_follower")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyShopViolation)
                .HasColumnName("notify_shop_violation")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyShopPenalty)
                .HasColumnName("notify_shop_penalty")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyPaymentReceived)
                .HasColumnName("notify_payment_received")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyWithdrawalCompleted)
                .HasColumnName("notify_withdrawal_completed")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifyPolicyUpdate)
                .HasColumnName("notify_policy_update")
                .HasDefaultValue(true);
            
            builder.Property(a => a.NotifySystemMaintenance)
                .HasColumnName("notify_system_maintenance")
                .HasDefaultValue(true);

            builder.Property(a => a.EmailEnabled)
                .HasColumnName("email_enabled")
                .HasDefaultValue(true);

            builder.Property(a => a.SmsEnabled)
                .HasColumnName("sms_enabled")
                .HasDefaultValue(false);

            builder.Property(a => a.PushEnabled)
                .HasColumnName("push_enabled")
                .HasDefaultValue(true);

            builder.Property(a => a.InAppEnabled)
                .HasColumnName("in_app_enabled")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableQuietHours)
                .HasColumnName("enable_quiet_hours")
                .HasDefaultValue(false);

            builder.Property(a => a.QuietHoursStart)
                .HasColumnName("quiet_hours_start")
                .IsRequired(false);

            builder.Property(a => a.QuietHoursEnd)
                .HasColumnName("quiet_hours_end")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.ShopId)
                .HasDatabaseName("idx_notification_settings_shop");
        }
    }
}
