namespace Daco.Infrastructure.Persistence.Configurations
{
    public class OrderVoucherConfiguration : IEntityTypeConfiguration<OrderVoucher>
    {
        public void Configure(EntityTypeBuilder<OrderVoucher> builder)
        {
            builder.ToTable("order_vouchers");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(a => a.VoucherId)
                .HasColumnName("voucher_id")
                .IsRequired();

            builder.Property(a => a.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<Order>()
               .WithMany()
               .HasForeignKey(a => a.OrderId)
               .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => a.OrderId)
                .HasDatabaseName("idx_order_vouchers_order");
        }
    }
}
