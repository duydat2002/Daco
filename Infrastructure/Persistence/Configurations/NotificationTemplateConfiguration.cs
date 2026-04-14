namespace Daco.Infrastructure.Persistence.Configurations
{
    public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
    {
        public void Configure(EntityTypeBuilder<NotificationTemplate> builder)
        {
            builder.ToTable("notification_templates");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.TemplateKey)
                .HasColumnName("template_key")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(a => a.TemplateKey)
                .IsUnique();

            builder.Property(a => a.TitleTemplate)
                .HasColumnName("title_template")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.MessageTemplate)
                .HasColumnName("message_template")
                .IsRequired();

            builder.Property(a => a.EmailSubjectTemplate)
                .HasColumnName("email_subject_template")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.EmailBodyTemplate)
                .HasColumnName("email_body_template")
                .IsRequired(false);

            builder.Property(a => a.SmsTemplate)
                .HasColumnName("sms_template")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(a => a.PushTitleTemplate)
                .HasColumnName("push_title_template")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.PushBodyTemplate)
                .HasColumnName("push_body_template")
                .IsRequired(false);

            builder.Property(a => a.AvailableVariables)
                .HasColumnName("available_variables")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.TemplateKey)
                .HasDatabaseName("idx_notification_templates_key");
        }
    }
}
