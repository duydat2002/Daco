namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerActivityLogConfiguration : IEntityTypeConfiguration<SellerActivityLog>
    {
        public void Configure(EntityTypeBuilder<SellerActivityLog> builder)
        {
            builder.ToTable("seller_activity_logs");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.ActivityType)
                .HasColumnName("activity_type")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.ActivityDescription)
                .HasColumnName("activity_description")
                .IsRequired(false);

            builder.Property(a => a.TargetType)
                .HasColumnName("target_type")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.TargetId)
                .HasColumnName("target_id")
                .IsRequired(false);

            builder.Property(a => a.OldValue)
                .HasColumnName("old_value")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.NewValue)
                .HasColumnName("new_value")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.IpAddress)
                .HasColumnName("ip_address")
                .HasColumnType("inet")
                .IsRequired(false);

            builder.Property(a => a.UserAgent)
                .HasColumnName("user_agent")
                .IsRequired(false);

            builder.Property(a => a.DeviceType)
                .HasColumnName("device_type")
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<Seller>()
               .WithMany()
               .HasForeignKey(a => a.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => new { a.SellerId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_seller_logs_seller");

            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_seller_logs_user");

            builder.HasIndex(a => new { a.ActivityType, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_seller_logs_activity");

            builder.HasIndex(a => new { a.TargetType, a.TargetId })
                .HasDatabaseName("idx_seller_logs_target");
        }
    }
}
