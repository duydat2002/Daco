namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShopChatSettingConfiguration : IEntityTypeConfiguration<ShopChatSetting>
    {
        public void Configure(EntityTypeBuilder<ShopChatSetting> builder)
        {
            builder.ToTable("shop_chat_settings");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.HasIndex(a => a.ShopId)
                .IsUnique();

            builder.Property(a => a.EnablePopupNotifications)
                .HasColumnName("enable_popup_notifications")
                .HasDefaultValue(true);

            builder.Property(a => a.EnableSoundNotifications)
                .HasColumnName("enable_sound_notifications")
                .HasDefaultValue(true);

            builder.Property(a => a.NotificationSound)
                .HasColumnName("notification_sound")
                .HasMaxLength(50)
                .HasDefaultValue("default");

            builder.Property(a => a.EnableAutoReply)
                .HasColumnName("enable_auto_reply")
                .HasDefaultValue(false);

            builder.Property(a => a.AutoReplyMessage)
                .HasColumnName("auto_reply_message")
                .IsRequired(false);

            builder.Property(a => a.AutoReplyDelaySeconds)
                .HasColumnName("auto_reply_delay_seconds")
                .HasDefaultValue(5);

            builder.Property(a => a.QuickReplies)
                .HasColumnName("quick_replies")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<QuickReply>>(v) ?? new List<QuickReply>()
                );

            builder.Property(a => a.WorkingHours)
                .HasColumnName("working_hours")
                .HasColumnType("jsonb")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<WorkingHours>(v) ?? null
                );

            builder.Property(a => a.OutsideHoursMessage)
                .HasColumnName("outside_hours_message")
                .IsRequired(false);

            builder.Property(a => a.AwayMode)
                .HasColumnName("away_mode")
                .HasDefaultValue(true);

            builder.Property(a => a.AwayMessage)
                .HasColumnName("away_message")
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.ShopId)
                .HasDatabaseName("idx_chat_settings_shop");
        }
    }
}
