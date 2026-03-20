namespace Daco.Infrastructure.Persistence.Configurations
{
    public class AdminRoleAssignmentConfiguration : IEntityTypeConfiguration<AdminRoleAssignment>
    {
        public void Configure(EntityTypeBuilder<AdminRoleAssignment> builder)
        {
            builder.ToTable("admin_role_assignments");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.AdminId)
                .HasColumnName("admin_id")
                .IsRequired();

            builder.Property(a => a.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.Property(a => a.AssignedBy)
                .HasColumnName("assigned_by")
                .IsRequired();

            builder.Property(a => a.AssignedAt)
                .HasColumnName("assigned_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired(false);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.HasIndex(a => a.AdminId)
                .HasDatabaseName("idx_admin_assignments_admin");

            builder.HasIndex(a => a.RoleId)
                .HasDatabaseName("idx_admin_assignments_role");
        }
    }
}
