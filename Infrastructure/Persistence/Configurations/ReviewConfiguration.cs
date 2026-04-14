namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(a => a.OrderItemId)
                .HasColumnName("order_item_id")
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.VariantId)
                .HasColumnName("variant_id")
                .IsRequired(false);

            builder.Property(a => a.Rating)
                .HasColumnName("rating")
                .IsRequired();

            builder.Property(a => a.Comment)
                .HasColumnName("comment")
                .IsRequired(false);

            builder.Property(a => a.Images)
                .HasColumnName("images")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.Videos)
                .HasColumnName("videos")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.IsVerifiedPurchase)
                .HasColumnName("is_verified_purchase")
                .HasDefaultValue(true);

            builder.Property(a => a.HelpfulCount)
                .HasColumnName("helpful_count")
                .HasDefaultValue(0);

            builder.Property(a => a.SellerReply)
                .HasColumnName("seller_reply")
                .IsRequired(false);

            builder.Property(a => a.SellerRepliedAt)
                .HasColumnName("seller_replied_at")
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(ReviewStatus.Pending);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Order>()
               .WithMany()
               .HasForeignKey(a => a.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<OrderItem>()
               .WithMany()
               .HasForeignKey(a => a.OrderItemId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Shop>()
               .WithMany()
               .HasForeignKey(a => a.ShopId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ProductVariant>()
               .WithMany()
               .HasForeignKey(a => a.VariantId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(p => p.ReviewReactions)
                .WithOne()
                .HasForeignKey(v => v.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ReviewReactions)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_reviewReactions");

            // Indexes
            builder.HasIndex(a => a.OrderItemId)
                .IsUnique();

            builder.HasIndex(a => new { a.ProductId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_reviews_product");

            builder.HasIndex(a => new { a.ShopId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_reviews_shop");

            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_reviews_user");

            builder.HasIndex(a => new { a.ProductId, a.Rating })
                .HasDatabaseName("idx_reviews_rating");
        }
    }
}
