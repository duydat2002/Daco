namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ViolationTypeConfiguration : IEntityTypeConfiguration<ViolationType>
    {
        public void Configure(EntityTypeBuilder<ViolationType> builder)
        {
            builder.ToTable("violation_types");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(a => a.Code)
                .IsUnique();

            builder.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.Severity)
                .HasColumnName("severity")
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(a => a.AutoHideProduct)
                .HasColumnName("auto_hide_product")
                .HasDefaultValue(false);

            builder.Property(a => a.AutoSuspendShop)
                .HasColumnName("auto_suspend_shop")
                .HasDefaultValue(false);

            builder.Property(a => a.PenaltyPoints)
                .HasColumnName("penalty_points")
                .HasDefaultValue(0);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);
        }
    }
}
