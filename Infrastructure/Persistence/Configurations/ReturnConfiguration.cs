namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ReturnConfiguration : IEntityTypeConfiguration<Return>
    {
        public void Configure(EntityTypeBuilder<Return> builder)
        {
            builder.ToTable("returns");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.ReturnCode)
                .HasColumnName("return_code")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(a => a.ReturnCode)
                .IsUnique();

            builder.Property(a => a.ReturnType)
                .HasColumnName("return_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.ReturnItems)
                .HasColumnName("return_items")
                .HasColumnType("jsonb")
                .IsRequired();

            builder.Property(a => a.RejectReason)
                .HasColumnName("reason")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.EvidenceImages)
                .HasColumnName("evidence_images")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.RefundAmount)
                .HasColumnName("refund_amount")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(ReturnStatus.Pending);

            builder.Property(a => a.ReviewedBy)
                .HasColumnName("reviewed_by")
                .IsRequired(false);

            builder.Property(a => a.ReviewedAt)
                .HasColumnName("reviewed_at")
                .IsRequired(false);

            builder.Property(a => a.RejectReason)
                .HasColumnName("reject_reason")
                .IsRequired(false);

            builder.Property(a => a.CompletedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(a => a.ApprovedAt)
                .HasColumnName("approved_at")
                .IsRequired(false);

            builder.Property(a => a.RefundedAt)
                .HasColumnName("refunded_at")
                .IsRequired(false);

            builder.Property(a => a.CompletedAt)
                .HasColumnName("completed_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Order>()
               .WithMany()
               .HasForeignKey(a => a.OrderId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Shop>()
               .WithMany()
               .HasForeignKey(a => a.ShopId)
               .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(a => a.OrderId)
                .HasDatabaseName("idx_returns_order");

            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_returns_user");

            builder.HasIndex(a => new { a.ShopId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_returns_shop");

            builder.HasIndex(a => a.Status)
                .HasDatabaseName("idx_returns_status");
        }
    }
}
