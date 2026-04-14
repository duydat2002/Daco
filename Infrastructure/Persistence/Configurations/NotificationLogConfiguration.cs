namespace Daco.Infrastructure.Persistence.Configurations
{
    public class NotificationLogConfiguration : IEntityTypeConfiguration<NotificationLog>
    {
        public void Configure(EntityTypeBuilder<NotificationLog> builder)
        {
            builder.ToTable("notification_logs");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.NotificationId)
                .HasColumnName("notification_id")
                .IsRequired();

            builder.Property(a => a.Channel)
                .HasColumnName("channel")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.RecipientEmail)
                .HasColumnName("recipient_email")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.RecipientPhone)
                .HasColumnName("recipient_phone")
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(a => a.RecipientDeviceToken)
                .HasColumnName("recipient_device_token")
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(DeliveryStatus.Pending);

            builder.Property(a => a.GatewayProvider)
                .HasColumnName("gateway_provider")
                .IsRequired();

            builder.Property(a => a.NotificationId)
                .HasColumnName("notification_id")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.GatewayMessageId)
                .HasColumnName("gateway_message_id")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.GatewayResponse)
                .HasColumnName("gateway_response")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.SentAt)
                .HasColumnName("sent_at")
                .IsRequired(false);

            builder.Property(a => a.DeliveredAt)
                .HasColumnName("delivered_at")
                .IsRequired(false);

            builder.Property(a => a.FailedAt)
                .HasColumnName("failed_at")
                .IsRequired(false);

            builder.Property(a => a.ErrorMessage)
                .HasColumnName("error_message")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<Notification>()
               .WithMany()
               .HasForeignKey(a => a.NotificationId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.NotificationId)
                .HasDatabaseName("idx_notification_logs_notification");

            builder.HasIndex(a => new { a.Status, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_notification_logs_status");

            builder.HasIndex(a => new { a.Channel, a.Status })
                .HasDatabaseName("idx_notification_logs_channel");
        }
    }
}
