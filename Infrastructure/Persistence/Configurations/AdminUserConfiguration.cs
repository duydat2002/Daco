namespace Daco.Infrastructure.Persistence.Configurations
{
    public class AdminUserConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> builder)
        {
            builder.ToTable("admin_users");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.EmployeeCode)
                .HasColumnName("employee_code")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.Department)
                .HasColumnName("department")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.Position)
                .HasColumnName("position")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.WorkEmail)
                .HasColumnName("work_email")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.WorkPhone)
                .HasColumnName("work_phone")
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(AdminStatus.Active);

            builder.Property(a => a.Notes)
                .HasColumnName("notes")
                .IsRequired(false);

            builder.Property(a => a.AssignedBy)
                .HasColumnName("assigned_by")
                .IsRequired(false);

            // Timestamps
            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(a => a.UserId)
                .IsUnique()
                .HasDatabaseName("idx_admin_users_user");

            builder.HasIndex(a => a.EmployeeCode)
                .IsUnique()
                .HasDatabaseName("idx_admin_users_code");

            builder.HasIndex(a => a.Status)
                .HasDatabaseName("idx_admin_users_status");

            // Collections
            builder.HasMany(a => a.RoleAssignments)
                .WithOne()
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.RoleAssignments)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_roleAssignments");

            builder.HasMany(a => a.CustomPermissions)
                .WithOne()
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.CustomPermissions)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_customPermissions");
        }
    }
}