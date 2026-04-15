namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ViolationActionsLogConfiguration : IEntityTypeConfiguration<ViolationActionsLog>
    {
        public void Configure(EntityTypeBuilder<ViolationActionsLog> builder)
        {
            builder.ToTable("violation_actions_log");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ViolationId)
                .HasColumnName("violation_id")
                .IsRequired();

            builder.Property(a => a.ActionType)
                .HasColumnName("action_type")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.ActionDetails)
                .HasColumnName("action_details")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.PerformedBy)
                .HasColumnName("performed_by")
                .IsRequired(false);

            builder.Property(a => a.PerformedAt)
                .HasColumnName("performed_at")
                .HasDefaultValueSql("NOW()");

            // Indexes
            builder.HasIndex(a => new { a.ViolationId, a.PerformedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_action_log_violation");
        }
    }
}
