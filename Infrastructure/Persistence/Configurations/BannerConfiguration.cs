namespace Daco.Infrastructure.Persistence.Configurations
{
    public class BannerConfiguration : IEntityTypeConfiguration<Banner>
    {
        public void Configure(EntityTypeBuilder<Banner> builder)
        {
            builder.ToTable("banners");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.BannerName)
                .HasColumnName("banner_name")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.BannerType)
                .HasColumnName("banner_type")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.ImageUrl)
                .HasColumnName("image_url")
                .IsRequired(false);

            builder.Property(a => a.ImageMobileUrl)
                .HasColumnName("image_mobile_url")
                .IsRequired(false);

            builder.Property(a => a.VideoUrl)
                .HasColumnName("video_url")
                .IsRequired(false);

            builder.Property(a => a.HtmlContent)
                .HasColumnName("html_content")
                .IsRequired(false);

            builder.Property(a => a.LinkUrl)
                .HasColumnName("link_url")
                .IsRequired(false);

            builder.Property(a => a.LinkTarget)
                .HasColumnName("link_target")
                .HasMaxLength(20)
                .HasDefaultValue(LinkTargets.Blank);

            builder.Property(a => a.Position)
                .HasColumnName("position")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.Width)
                .HasColumnName("width")
                .IsRequired(false);

            builder.Property(a => a.Height)
                .HasColumnName("height")
                .IsRequired(false);

            builder.Property(a => a.TargetAudience)
                .HasColumnName("target_audience")
                .HasColumnType("jsonb")
                .IsRequired(false);

            builder.Property(a => a.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(a => a.StartDate)
                .HasColumnName("start_date")
                .IsRequired(false);

            builder.Property(a => a.EndDate)
                .HasColumnName("end_date")
                .IsRequired(false);

            builder.Property(a => a.ViewCount)
                .HasColumnName("view_count")
                .HasDefaultValue(0);

            builder.Property(a => a.ClickCount)
                .HasColumnName("click_count")
                .HasDefaultValue(0);

            builder.Property(a => a.AltText)
                .HasColumnName("alt_text")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<AdminUser>()
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes 
            builder.HasIndex(a => new {a.Position, a.SortOrder})
                .HasFilter("is_active = TRUE")
                .HasDatabaseName("idx_banners_position");

            builder.HasIndex(a => new { a.IsActive, a.StartDate, a.EndDate })
                .HasDatabaseName("idx_banners_active");
        }
    }
}
