namespace Daco.Infrastructure.Persistence.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.ToTable("vouchers");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.VoucherCode)
                .HasColumnName("voucher_code")
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(a => a.VoucherCode)
                .IsUnique();

            builder.Property(a => a.VoucherType)
                .HasColumnName("voucher_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired(false);

            builder.Property(a => a.DiscountType)
                .HasColumnName("discount_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.DiscountValue)
                .HasColumnName("discount_value")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired();

            builder.Property(a => a.MaxDiscountAmount)
                .HasColumnName("max_discount_amount")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired(false);

            builder.Property(a => a.MinOrderValue)
                .HasColumnName("min_order_value")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.ApplicableCategories)
                .HasColumnName("applicable_categories")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.ApplicableProducts)
                .HasColumnName("applicable_products")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.TotalQuantity)
                .HasColumnName("total_quantity")
                .IsRequired(false);

            builder.Property(a => a.UsedQuantity)
                .HasColumnName("used_quantity")
                .HasDefaultValue(0);

            builder.Property(a => a.MaxUsagePerUser)
                .HasColumnName("max_usage_per_user")
                .HasDefaultValue(1);

            builder.Property(a => a.StartDate)
                .HasColumnName("start_date")
                .IsRequired();

            builder.Property(a => a.EndDate)
                .HasColumnName("end_date")
                .IsRequired();

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Shop>()
               .WithMany()
               .HasForeignKey(a => a.ShopId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.UserVouchers)
               .WithOne()
               .HasForeignKey(v => v.VoucherId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.UserVouchers)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_userVouchers");

            builder.HasMany(p => p.OrderVouchers)
               .WithOne()
               .HasForeignKey(v => v.VoucherId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.OrderVouchers)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_orderVouchers");

            // Indexes
            builder.HasIndex(a => a.VoucherCode)
                .HasFilter("is_active = TRUE")
                .HasDatabaseName("idx_vouchers_code");

            builder.HasIndex(a => a.ShopId)
                .HasFilter("is_active = TRUE")
                .HasDatabaseName("idx_vouchers_shop");

            builder.HasIndex(a => new { a.StartDate, a.EndDate })
                .HasFilter("is_active = TRUE")
                .HasDatabaseName("idx_vouchers_code");
        }
    }
}
