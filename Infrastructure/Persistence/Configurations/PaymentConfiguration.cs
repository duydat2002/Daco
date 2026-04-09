namespace Daco.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");

            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(s => s.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(s => s.PaymentMethod)
                .HasColumnName("payment_method")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(s => s.Amount)
                .HasColumnName("amount")
                .HasColumnType("decimal(15,2)")
                .IsRequired();

            builder.Property(s => s.PaymentGateway)
               .HasColumnName("payment_gateway")
               .HasMaxLength(100)
               .IsRequired(false);

            builder.Property(s => s.GatewayTransactionId)
               .HasColumnName("gateway_transaction_id")
               .HasMaxLength(255)
               .IsRequired(false);

            builder.Property(s => s.GatewayResponse)
               .HasColumnName("gateway_response")
               .HasColumnType("jsonb")
               .IsRequired(false);

            builder.Property(s => s.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(PaymentStatus.Unpaid);

            // Timestamps
            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(s => s.PaidAt)
                .HasColumnName("paid_at")
                .IsRequired(false);

            builder.Property(s => s.FailedAt)
                .HasColumnName("failed_at")
                .IsRequired(false);

            builder.Property(s => s.FailureReason)
                .HasColumnName("failure_reason")
                .IsRequired(false);

            // Indexes
            builder.HasIndex(s => s.OrderId)
                .HasDatabaseName("idx_payments_order");

            builder.HasIndex(s => s.Status)
                .HasDatabaseName("idx_payments_status");

            builder.HasIndex(s => s.GatewayTransactionId)
                .HasDatabaseName("idx_payments_gateway_txn");

            // Relation
            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
