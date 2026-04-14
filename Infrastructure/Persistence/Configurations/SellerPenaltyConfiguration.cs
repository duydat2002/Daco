namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerPenaltyConfiguration : IEntityTypeConfiguration<SellerPenalty>
    {
        public void Configure(EntityTypeBuilder<SellerPenalty> builder)
        {
            builder.ToTable("seller_penalties");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
                .IsRequired();

            builder.Property(a => a.ViolationId)
                .HasColumnName("violation_id")
                .IsRequired(false);

            builder.Property(a => a.PenaltyType)
                .HasColumnName("penalty_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.Reason)
                .HasColumnName("reason")
                .IsRequired();

            builder.Property(a => a.PenaltyPoints)
                .HasColumnName("penalty_points")
                .HasDefaultValue(0);

            builder.Property(a => a.FineAmount)
                .HasColumnName("fine_amount")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired(false);

            builder.Property(a => a.SuspendFrom)
                .HasColumnName("suspend_from")
                .IsRequired(false);

            builder.Property(a => a.SuspendTo)
                .HasColumnName("suspend_to")
                .IsRequired(false);

            builder.Property(a => a.IssuedBy)
                .HasColumnName("issued_by")
                .IsRequired(false);

            builder.Property(a => a.AppealNote)
                .HasColumnName("appeal_note")
                .IsRequired(false);

            builder.Property(a => a.AppealSubmittedAt)
                .HasColumnName("appeal_submitted_at")
                .IsRequired(false);

            builder.Property(a => a.AppealStatus)
                .HasColumnName("appeal_status")
                .HasConversion<int>()
                .IsRequired(false);

            builder.Property(a => a.AppealReviewedBy)
                .HasColumnName("appeal_reviewed_by")
                .IsRequired(false);

            builder.Property(a => a.AppealReviewedAt)
                .HasColumnName("appeal_reviewed_at")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<Seller>()
               .WithMany()
               .HasForeignKey(a => a.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ProductViolation>()
               .WithMany()
               .HasForeignKey(a => a.ViolationId)
               .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => new { a.SellerId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_penalties_seller");

            builder.HasIndex(a => a.ViolationId)
                .HasDatabaseName("idx_penalties_violation");

            builder.HasIndex(a => a.AppealStatus)
                .HasFilter("appeal_status = 'pending'")
                .HasDatabaseName("idx_penalties_appeal");
        }
    }
}
