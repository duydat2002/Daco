namespace Daco.Infrastructure.Persistence.Configurations
{
    public class UserReportConfiguration : IEntityTypeConfiguration<UserReport>
    {
        public void Configure(EntityTypeBuilder<UserReport> builder)
        {
            builder.ToTable("user_reports");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ReporterId)
                .HasColumnName("reporter_id")
                .IsRequired();

            builder.Property(a => a.EntityType)
                .HasColumnName("entity_type")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.EntityId)
                .HasColumnName("entity_id")
                .IsRequired();

            builder.Property(a => a.Reason)
                .HasColumnName("reason")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.EvidenceUrls)
                .HasColumnName("evidence_urls")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(ReportStatus.Pending);

            builder.Property(a => a.AssignedTo)
                .HasColumnName("assigned_to")
                .IsRequired(false);

            builder.Property(a => a.ReviewedAt)
                .HasColumnName("reviewed_at")
                .IsRequired(false);

            builder.Property(a => a.ReviewNotes)
                .HasColumnName("review_notes")
                .IsRequired(false);

            builder.Property(a => a.ActionTaken)
                .HasColumnName("action_taken")
                .IsRequired(false);

            builder.Property(a => a.ResolvedAt)
                .HasColumnName("resolved_at")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.ReporterId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.EntityType, a.EntityId })
                .HasDatabaseName("idx_user_reports_entity");

            builder.HasIndex(a => new { a.Status, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_user_reports_status");

            builder.HasIndex(a => a.AssignedTo)
                .HasFilter("status IN (0, 1)")
                .HasDatabaseName("idx_user_reports_assigned");
        }
    }
}
