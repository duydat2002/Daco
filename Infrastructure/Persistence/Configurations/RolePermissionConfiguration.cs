namespace Daco.Infrastructure.Persistence.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("role_permissions");

            builder.HasKey(rp => rp.Id);
            builder.Property(rp => rp.Id)
                .HasColumnName("id");

            builder.Property(rp => rp.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.Property(rp => rp.PermissionId)
                .HasColumnName("permission_id")
                .IsRequired();

            builder.Property(rp => rp.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(rp => rp.RoleId)
                .HasDatabaseName("idx_role_permissions_role");

            builder.HasIndex(rp => rp.PermissionId)
                .HasDatabaseName("idx_role_permissions_permission");
        }
    }
}
