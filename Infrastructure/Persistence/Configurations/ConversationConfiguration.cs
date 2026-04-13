namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("conversations");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.BuyerId)
                .HasColumnName("buyer_id")
                .IsRequired();

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.LastMessageId)
                .HasColumnName("last_message_id")
                .IsRequired(false);

            builder.Property(a => a.LastMessageAt)
                .HasColumnName("last_message_at")
                .IsRequired(false);

            builder.Property(a => a.LastMessagePreview)
                .HasColumnName("last_message_preview")
                .IsRequired(false);

            builder.Property(a => a.UnreadCountBuyer)
                .HasColumnName("unread_count_buyer")
                .HasDefaultValue(0);

            builder.Property(a => a.UnreadCountShop)
                .HasColumnName("unread_count_shop")
                .HasDefaultValue(0);

            builder.Property(a => a.IsBlocked)
                .HasColumnName("is_blocked")
                .HasDefaultValue(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasMany(a => a.Messages)
                .WithOne()
                .HasForeignKey(a => a.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.Messages)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_messages");

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Shop>()
                .WithMany()
                .HasForeignKey(c => c.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.BuyerId, a.ShopId })
                .IsUnique();

            builder.HasIndex(c => new { c.BuyerId, c.LastMessageAt })
                .HasDatabaseName("idx_conversations_buyer");

            builder.HasIndex(c => new { c.ShopId, c.LastMessageAt })
                .HasDatabaseName("idx_conversations_shop");

            builder.HasIndex(c => new { c.BuyerId, c.UnreadCountBuyer })
                .HasFilter("unread_count_buyer > 0")
                .HasDatabaseName("idx_conversations_unread_buyer");

            builder.HasIndex(c => new { c.ShopId, c.UnreadCountShop })
                .HasFilter("unread_count_shop > 0")
                .HasDatabaseName("idx_conversations_unread_shop");
        }
    }
}
