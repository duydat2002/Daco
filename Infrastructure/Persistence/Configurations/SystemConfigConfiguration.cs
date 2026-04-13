namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SystemConfigConfiguration : IEntityTypeConfiguration<SystemConfig>
    {
        public void Configure(EntityTypeBuilder<SystemConfig> builder)
        {
            builder.ToTable("system_configs");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ConfigKey)
                .HasColumnName("config_key")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(a => a.ConfigKey)
                .IsUnique();

            builder.Property(a => a.ConfigValue)
                .HasColumnName("config_value")
                .IsRequired();

             builder.Property(a => a.ValueType)
                .HasColumnName("value_type")
                .HasConversion<int>()
                .HasDefaultValue(ConfigValueType.String);

            builder.Property(a => a.Category)
               .HasColumnName("category")
               .HasMaxLength(100)
               .IsRequired(false);

            builder.Property(a => a.Description)
               .HasColumnName("description")
               .IsRequired(false);

            builder.Property(a => a.ValidationRules)
               .HasColumnName("validation_rules")
               .HasColumnType("jsonb")
               .IsRequired(false);

            builder.Property(a => a.IsPublic)
               .HasColumnName("is_public")
               .HasDefaultValue(false);

            builder.Property(a => a.IsEditable)
               .HasColumnName("is_editable")
               .HasDefaultValue(true);

            builder.Property(a => a.UpdatedBy)
               .HasColumnName("updated_by")
               .IsRequired(false);

            builder.Property(a => a.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired(false);

            builder.Property(a => a.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("NOW()");

            // FK 
            builder.HasOne<AdminUser>()
                .WithMany()
                .HasForeignKey(c => c.UpdatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => a.ConfigKey)
              .HasDatabaseName("idx_system_configs_key");

            builder.HasIndex(a => a.Category)
              .HasDatabaseName("idx_system_configs_category");
        }
    }
}
