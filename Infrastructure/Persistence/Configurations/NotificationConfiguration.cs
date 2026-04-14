namespace Daco.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notifications");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired(false);

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired(false);

            builder.Property(a => a.NotificationType)
                .HasColumnName("notification_type")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.Priority)
                .HasColumnName("priority")
                .HasConversion<int>()
                .HasDefaultValue(NotificationPriority.Low);

            builder.Property(a => a.Title)
                .HasColumnName("title")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Message)
                .HasColumnName("message")
                .IsRequired();

            builder.Property(a => a.ActionUrl)
                .HasColumnName("action_url")
                .IsRequired(false);

            builder.Property(a => a.ActionLabel)
                .HasColumnName("action_label")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.ReferenceType)
                .HasColumnName("reference_type")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.ReferenceId)
                .HasColumnName("reference_id")
                .IsRequired(false);

            builder.Property(a => a.ImageUrl)
                .HasColumnName("image_url")
                .IsRequired(false);

            builder.Property(a => a.Icon)
                .HasColumnName("icon")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.IsRead)
                .HasColumnName("is_read")
                .HasDefaultValue(false);

            builder.Property(a => a.ReadAt)
                .HasColumnName("read_at")
                .IsRequired(false);

            builder.Property(a => a.SentVia)
                .HasColumnName("sent_via")
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'[]'::jsonb");

            builder.Property(a => a.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Shop>()
                .WithMany()
                .HasForeignKey(a => a.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasFilter("user_id IS NOT NULL")
                .HasDatabaseName("idx_notifications_user");

            builder.HasIndex(a => new { a.ShopId, a.CreatedAt })
                .IsDescending(false, true)
                .HasFilter("shop_id IS NOT NULL")
                .HasDatabaseName("idx_notifications_shop");

            builder.HasIndex(a => new { a.UserId, a.IsRead })
                .HasFilter("is_read = FALSE AND user_id IS NOT NULL")
                .HasDatabaseName("idx_notifications_unread_user");

            builder.HasIndex(a => new { a.ShopId, a.IsRead })
                .HasFilter("is_read = FALSE AND shop_id IS NOT NULL")
                .HasDatabaseName("idx_notifications_unread_shop");

            builder.HasIndex(a => new { a.NotificationType, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_notifications_type");
        }
    }
}
