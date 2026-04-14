namespace Daco.Infrastructure.Persistence.Configurations
{
    public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.ToTable("order_status_history");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.Note)
                .HasColumnName("note")
                .IsRequired(false);

            builder.Property(a => a.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<Order>()
               .WithMany()
               .HasForeignKey(a => a.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.OrderId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_order_history_order");
        }
    }
}
