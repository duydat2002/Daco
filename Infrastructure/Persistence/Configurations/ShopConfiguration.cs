namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.ToTable("shops");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SellerId)
                .HasColumnName("seller_id")
                .IsRequired();

            builder.HasIndex(a => a.SellerId)
                .IsUnique();

            builder.Property(a => a.ShopName)
                .HasColumnName("shop_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.ShopSlug)
                .HasColumnName("shop_slug")
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(a => a.ShopSlug)
                .IsUnique();

            builder.Property(a => a.ShopLogo)
                .HasColumnName("shop_logo")
                .IsRequired(false);

            builder.Property(a => a.ShopCover)
                .HasColumnName("shop_cover")
                .IsRequired(false);

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.ShopEmail)
                .HasColumnName("shop_email")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.ShopPhone)
                .HasColumnName("shop_phone")
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(ShopStatus.Active);

            builder.Property(a => a.ShopType)
                .HasColumnName("shop_type")
                .HasConversion<int>()
                .HasDefaultValue(ShopType.Normal);

            builder.Property(a => a.IsOfficial)
                .HasColumnName("is_official")
                .HasDefaultValue(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.JoinedAt)
                .HasColumnName("joined_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(a => a.DeletedAt)
                .HasColumnName("deleted_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Seller>()
               .WithMany()
               .HasForeignKey(a => a.SellerId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ShopAddresses)
                .WithOne()
                .HasForeignKey(v => v.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ShopAddresses)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_shopAddresses");

            builder.HasOne(p => p.ShopMetrics)
                .WithOne()
                .HasForeignKey<ShopMetrics>(v => v.ShopId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.ShopMetrics)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(p => p.ShopChatSetting)
               .WithOne()
               .HasForeignKey<ShopChatSetting>(v => v.ShopId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.ShopChatSetting)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(p => p.ShopNotificationSetting)
               .WithOne()
               .HasForeignKey<ShopNotificationSetting>(v => v.ShopId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.ShopNotificationSetting)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            // Indexes
            builder.HasIndex(a => a.SellerId)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_shops_seller");

            builder.HasIndex(a => a.ShopSlug)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_shops_slug");

            builder.HasIndex(a => a.Status)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_shops_status");

            builder.HasIndex(a => a.ShopType)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_shops_type");
        }
    }
}
