namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShopFollowerConfiguration : IEntityTypeConfiguration<ShopFollower>
    {
        public void Configure(EntityTypeBuilder<ShopFollower> builder)
        {
            builder.ToTable("shop_followers");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.NotifyNewProducts)
                .HasColumnName("notify_new_products")
                .HasDefaultValue(true);

            builder.Property(a => a.NotifyPromotions)
                .HasColumnName("notify_promotions")
                .HasDefaultValue(true);

            builder.Property(a => a.NotifyFlashSales)
                .HasColumnName("notify_flash_sales")
                .HasDefaultValue(true);

            builder.Property(a => a.NotifyLivestream)
                .HasColumnName("notify_livestream")
                .HasDefaultValue(false);

            builder.Property(a => a.FollowedAt)
                .HasColumnName("followed_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UnfollowedAt)
                .HasColumnName("unfollowed_at")
                .IsRequired(false);

            // FK
            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.ShopId, a.UserId })
                .IsUnique();

            builder.HasIndex(a => new { a.ShopId, a.FollowedAt })
                .IsDescending(false, true)
                .HasFilter("unfollowed_at IS NULL")
                .HasDatabaseName("idx_shop_followers_shop");

            builder.HasIndex(a => new { a.UserId, a.FollowedAt })
                .IsDescending(false, true)
                .HasFilter("unfollowed_at IS NULL")
                .HasDatabaseName("idx_shop_followers_user");
        }
    }
}
