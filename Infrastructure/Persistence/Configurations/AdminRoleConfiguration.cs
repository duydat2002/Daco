namespace Daco.Infrastructure.Persistence.Configurations
{
    public class AdminRoleConfiguration : IEntityTypeConfiguration<AdminRole>
    {
        public void Configure(EntityTypeBuilder<AdminRole> builder)
        {
            builder.ToTable("admin_roles");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasColumnName("id");

            builder.Property(r => r.RoleCode)
                .HasColumnName("role_code")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.RoleName)
                .HasColumnName("role_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(r => r.Level)
                .HasColumnName("level")
                .IsRequired();

            builder.Property(r => r.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(r => r.IsSystem)
                .HasColumnName("is_system")
                .HasDefaultValue(false);
        }
    }
}
