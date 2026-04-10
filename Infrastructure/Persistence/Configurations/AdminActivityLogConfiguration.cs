namespace Daco.Infrastructure.Persistence.Configurations
{
    public class AdminActivityLogConfiguration : IEntityTypeConfiguration<AdminActivityLog>
    {
        public void Configure(EntityTypeBuilder<AdminActivityLog> builder)
        {
            builder.ToTable("admin_activity_logs");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.AdminId)
                .HasColumnName("admin_id")
                .IsRequired(false);

            builder.Property(a => a.ActionType)
                .HasColumnName("action_type")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.ActionDescription)
                .HasColumnName("action_description")
                .IsRequired(false);

            builder.Property(a => a.TargetType)
                .HasColumnName("target_type")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.TargetId)
                .HasColumnName("target_id")
                .IsRequired(false);

            builder.Property(a => a.ActionDescription)
                .HasColumnName("action_description")
                .IsRequired(false);

            builder.Property(a => a.OldValue)
                .HasColumnName("old_value")
                .IsRequired(false);

            builder.Property(a => a.NewValue)
                .HasColumnName("new_value")
                .IsRequired(false);

            builder.Property(a => a.IpAddress)
                .HasColumnName("ip_address")
                .HasColumnType("inet")
                .IsRequired(false);

            builder.Property(a => a.UserAgent)
                .HasColumnName("user_agent")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()"); ;

            builder.HasOne<AdminUser>()             
                .WithMany()
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.SetNull);

            //Indexes
            builder.HasIndex(a => new { a.AdminId, a.CreatedAt })
               .IsDescending(false, true)
               .HasDatabaseName("idx_admin_logs_admin");

            builder.HasIndex(a => new { a.ActionType, a.CreatedAt })
               .IsDescending(false, true)
               .HasDatabaseName("idx_admin_logs_action");

            builder.HasIndex(a => new { a.TargetType, a.TargetId })
               .HasDatabaseName("idx_admin_logs_target");
        }
    }
}
