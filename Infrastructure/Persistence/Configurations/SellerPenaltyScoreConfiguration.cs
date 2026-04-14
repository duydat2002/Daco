namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SellerPenaltyScoreConfiguration : IEntityTypeConfiguration<SellerPenaltyScore>
    {
        public void Configure(EntityTypeBuilder<SellerPenaltyScore> builder)
        {
            builder.ToTable("seller_penalty_scores");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
                .IsRequired();

            builder.HasIndex(a => a.SellerId)
                .IsUnique();

            builder.Property(a => a.TotalPoints)
                .HasColumnName("total_points")
                .HasDefaultValue(0);

            builder.Property(a => a.WarningCount)
                .HasColumnName("warning_count")
                .HasDefaultValue(0);

            builder.Property(a => a.ViolationCount)
                .HasColumnName("violation_count")
                .HasDefaultValue(0);

            builder.Property(a => a.LastViolationAt)
                .HasColumnName("last_violation_at")
                .IsRequired(false);

            builder.Property(a => a.RiskLevel)
                .HasColumnName("risk_level")
                .HasConversion<int>()
                .HasDefaultValue(RiskLevel.Low);

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<Seller>()
               .WithMany()
               .HasForeignKey(a => a.SellerId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => a.RiskLevel)
                .HasDatabaseName("idx_penalty_scores_risk");
        }
    }
}
