namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductViolationConfiguration : IEntityTypeConfiguration<ProductViolation>
    {
        public void Configure(EntityTypeBuilder<ProductViolation> builder)
        {
            builder.ToTable("product_violations");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.ViolationTypeId)
                .HasColumnName("violation_type_id")
                .IsRequired();

            builder.Property(a => a.Source)
                .HasColumnName("source")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.ReporterId)
                .HasColumnName("reporter_id")
                .IsRequired(false);

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired();

            builder.Property(a => a.EvidenceUrls)
                .HasColumnName("evidence_urls")
                .IsRequired();

            builder.Property(a => a.ViolationTypeId)
                .HasColumnName("violation_type_id")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(ViolationStatus.Pending);

            builder.Property(a => a.ReviewedBy)
                .HasColumnName("reviewed_by")
                .IsRequired(false);

            builder.Property(a => a.ReviewedAt)
                .HasColumnName("reviewed_at")
                .IsRequired(false);

            builder.Property(a => a.ReviewNotes)
                .HasColumnName("review_notes")
                .IsRequired(false);

            builder.Property(a => a.ActionsTaken)
                .HasColumnName("actions_taken")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.ProductSnapshot)
                .HasColumnName("product_snapshot")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.ResolvedBy)
                .HasColumnName("resolved_by")
                .IsRequired(false);

            builder.Property(a => a.ResolvedAt)
                .HasColumnName("resolved_at")
                .IsRequired(false);

            builder.Property(a => a.ResolutionNotes)
                .HasColumnName("resolution_notes")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Shop>()
               .WithMany()
               .HasForeignKey(a => a.ShopId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ViolationType>()
               .WithMany()
               .HasForeignKey(a => a.ViolationTypeId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.ReporterId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ViolationActionsLogs)
                .WithOne()
                .HasForeignKey(v => v.ViolationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ViolationActionsLogs)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_violationActionsLogs");

            // Indexes
            builder.HasIndex(a => a.ProductId)
                .HasDatabaseName("idx_violations_product");

            builder.HasIndex(a => new { a.ShopId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_violations_shop");

            builder.HasIndex(a => new { a.Status, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_violations_status");

            builder.HasIndex(a => a.ViolationTypeId)
                .HasDatabaseName("idx_violations_type");

            builder.HasIndex(a => a.ReviewedBy)
                .HasDatabaseName("idx_violations_reviewer");
        }
    }
}
