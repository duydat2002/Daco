namespace Daco.Infrastructure.Persistence.Configurations
{
    public class AdminPermissionConfiguration : IEntityTypeConfiguration<AdminPermission>
    {
        public void Configure(EntityTypeBuilder<AdminPermission> builder)
        {
            builder.ToTable("admin_permissions");

            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("id");

            builder.Property(p => p.PermissionName)
                .HasColumnName("permission_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.PermissionCode)
                .HasColumnName("permission_code")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Module)
                .HasColumnName("module")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(p => p.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(p => p.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.HasIndex(p => p.PermissionCode)
                .IsUnique()
                .HasDatabaseName("idx_admin_permissions_code");

            builder.HasIndex(p => p.Module)
                .HasDatabaseName("idx_admin_permissions_module");
        }
    }
}
