namespace Daco.Infrastructure.Persistence.Configurations
{
    public class AdminCustomPermissionConfiguration : IEntityTypeConfiguration<AdminCustomPermission>
    {
        public void Configure(EntityTypeBuilder<AdminCustomPermission> builder)
        {
            builder.ToTable("admin_custom_permissions");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.AdminId)
                .HasColumnName("admin_id")
                .IsRequired();

            builder.Property(a => a.PermissionId)
                .HasColumnName("permission_id")
                .IsRequired();

            builder.Property(a => a.PermissionCode)
                .HasColumnName("permission_code")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.IsGranted)
                .HasColumnName("is_granted")
                .HasDefaultValue(true);

            builder.Property(a => a.GrantedBy)
                .HasColumnName("granted_by")
                .IsRequired();

            builder.Property(a => a.Reason)
                .HasColumnName("reason")
                .IsRequired(false);

            builder.Property(a => a.GrantedAt)
                .HasColumnName("granted_at")
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(a => a.AdminId)
                .HasDatabaseName("idx_admin_custom_admin");
        }
    }
}
