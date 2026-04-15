namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ShopMetricsConfiguration : IEntityTypeConfiguration<ShopMetrics>
    {
        public void Configure(EntityTypeBuilder<ShopMetrics> builder)
        {
            builder.ToTable("shop_metrics");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.HasIndex(a => a.ShopId)
                .IsUnique();

            builder.Property(a => a.FollowerCount)
                .HasColumnName("follower_count")
                .HasDefaultValue(0);

            builder.Property(a => a.RatingCount)
                .HasColumnName("rating_count")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalProducts)
                .HasColumnName("total_products")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalOrders)
                .HasColumnName("total_orders")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalSold)
                .HasColumnName("total_sold")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalRevenue)
                .HasColumnName("total_revenue")
                .HasColumnType("DECIMAL(15,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.RatingAverage)
                .HasColumnName("rating_average")
                .HasColumnType("NUMERIC(3, 2)")
                .HasDefaultValue(0);

            builder.Property(a => a.TotalRatings)
                .HasColumnName("total_ratings")
                .HasDefaultValue(0);

            builder.Property(a => a.ResponseRate)
                .HasColumnName("response_rate")
                .HasColumnType("NUMERIC(5, 2)")
                .HasDefaultValue(0);

            builder.Property(a => a.ResponseTime)
                .HasColumnName("response_time")
                .HasColumnType("NUMERIC(3, 2)")
                .HasDefaultValue(0);

            builder.Property(a => a.ResponseTimeHours)
                .HasColumnName("response_time_hours")
                .IsRequired(false);
        }
    }
}
