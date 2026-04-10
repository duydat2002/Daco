using Daco.Domain.Analytics.Entities;

namespace Daco.Infrastructure.Persistence.Configurations
{
    public class PopularSearchConfiguration : IEntityTypeConfiguration<PopularSearch>
    {
        public void Configure(EntityTypeBuilder<PopularSearch> builder)
        {
            builder.ToTable("admin_activity_logs");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.SearchQuery)
                .HasColumnName("search_query")
                .IsRequired();

            builder.Property(a => a.SearchCount)
                .HasColumnName("search_count")
                .HasDefaultValue(0);

            builder.Property(a => a.ClickCount)
                .HasColumnName("click_count")
                .HasDefaultValue(0);

            builder.Property(a => a.ConversionCount)
                .HasColumnName("conversion_count")
                .HasDefaultValue(0);

            builder.Property(a => a.PeriodType)
                .HasColumnName("period_type")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(a => a.PeriodStart)
                .HasColumnName("period_start")
                .IsRequired();

            builder.Property(a => a.PeriodEnd)
                .HasColumnName("period_end")
                .IsRequired();

            builder.Property(a => a.Rank)
                .HasColumnName("rank")
                .IsRequired(false);

            builder.Property(a => a.LastUpdated)
                .HasColumnName("last_updated")
                .HasDefaultValueSql("NOW()");

            builder.HasIndex(x => new
            {
                x.SearchQuery,
                x.PeriodType,
                x.PeriodStart
            })
                .IsUnique()
                .HasDatabaseName("uq_popular_search");

            //Indexes
            builder.HasIndex(a => new { a.PeriodType, a.PeriodStart })
               .IsDescending(false, true)
               .HasDatabaseName("idx_popular_searches_period");

            builder.HasIndex(a => new { a.PeriodType, a.Rank })
               .HasDatabaseName("idx_popular_searches_rank");
        }
    }
}
