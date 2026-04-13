namespace Daco.Infrastructure.Persistence.Configurations
{
    public class SearchHistoryConfiguration : IEntityTypeConfiguration<SearchHistory>
    {
        public void Configure(EntityTypeBuilder<SearchHistory> builder)
        {
            builder.ToTable("search_history");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired(false);

            builder.Property(a => a.SessionId)
                .HasColumnName("session_id")
                .IsRequired(false);

            builder.Property(a => a.SearchQuery)
                .HasColumnName("search_query")
                .IsRequired();

            builder.Property(a => a.SearchType)
                .HasColumnName("search_type")
                .HasMaxLength(50)
                .HasDefaultValue("product");

            builder.Property(a => a.Filters)
                .HasColumnName("filters")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.ResultsCount)
                .HasColumnName("results_count")
                .HasDefaultValue(0);

            builder.Property(a => a.ClickedProductId)
                .HasColumnName("clicked_product_id")
                .IsRequired(false);

            builder.Property(a => a.ClickedPosition)
                .HasColumnName("clicked_position")
                .IsRequired(false);

            builder.Property(a => a.Source)
                .HasColumnName("source")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(a => a.DeviceType)
               .HasColumnName("device_type")
               .HasMaxLength(20)
               .IsRequired(false);

            builder.Property(a => a.IpAddress)
                .HasColumnName("ip_address")
                .HasColumnType("inet")
                .IsRequired(false);

            builder.Property(a => a.DeviceType)
               .HasColumnName("device_type")
               .HasMaxLength(20)
               .IsRequired(false);

            builder.Property(a => a.CountryCode)
               .HasColumnName("country_code")
               .HasMaxLength(2)
               .IsRequired(false);

            builder.Property(a => a.City)
               .HasColumnName("city")
               .HasMaxLength(100)
               .IsRequired(false);

            builder.Property(a => a.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(a => a.ClickedProductId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
              .IsDescending(false, true)
              .HasDatabaseName("idx_search_history_user");

            builder.HasIndex(a => new { a.SessionId, a.CreatedAt })
              .IsDescending(false, true)
              .HasDatabaseName("idx_search_history_session");

            builder.HasIndex(a => new { a.SearchQuery, a.CreatedAt })
              .IsDescending(false, true)
              .HasDatabaseName("idx_search_history_query");

            builder.HasIndex(a => a.CreatedAt)
              .IsDescending(true)
              .HasDatabaseName("idx_search_history_time");

            builder.HasIndex(a => new {a.SearchQuery, a.CreatedAt})
             .IsDescending(false, true)
             .HasFilter("created_at > NOW() - INTERVAL '7 days'")
             .HasDatabaseName("idx_search_history_trending");
        }
    }
}
