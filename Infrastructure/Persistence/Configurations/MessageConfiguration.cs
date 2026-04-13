namespace Daco.Infrastructure.Persistence.Configurations
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("messages");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SenderType)
                .HasColumnName("sender_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.SenderId)
                .HasColumnName("sender_id")
                .IsRequired(false);

            builder.Property(a => a.MessageType)
                .HasColumnName("message_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.Content)
                .HasColumnName("content")
                .IsRequired(false);

            builder.Property(a => a.Attachments)
                .HasColumnName("attachments")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired(false);

            builder.Property(a => a.OrderId)
                .HasColumnName("order_id")
                .IsRequired(false);

            builder.Property(a => a.VoucherId)
                .HasColumnName("voucher_id")
                .IsRequired(false);

            builder.Property(a => a.IsRead)
                .HasColumnName("is_read")
                .HasDefaultValue(false);

            builder.Property(a => a.ReadAt)
                .HasColumnName("read_at")
                .IsRequired(false);

            builder.Property(a => a.DeletedByBuyer)
                .HasColumnName("deleted_by_buyer")
                .IsRequired(false);

            builder.Property(a => a.DeletedByShop)
                .HasColumnName("deleted_by_shop")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(c => c.ProductId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(c => c.OrderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Voucher>()
                .WithMany()
                .HasForeignKey(c => c.VoucherId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(c => new { c.ConversationId, c.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_messages_conversation");

            builder.HasIndex(c => new { c.ConversationId, c.IsRead })
                .HasFilter("is_read = FALSE")
                .HasDatabaseName("idx_messages_unread");
        }
    }
}
