namespace Daco.Infrastructure.Persistence.Configurations
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable("pages");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.PageTitle)
                .HasColumnName("page_title")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.PageSlug)
                .HasColumnName("page_slug")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.HasIndex(a => a.PageSlug)
                .IsUnique();

            builder.Property(a => a.Content)
                .HasColumnName("content")
                .IsRequired();

            builder.Property(a => a.Excerpt)
                .HasColumnName("excerpt")
                .IsRequired(false);

            builder.Property(a => a.Template)
                .HasColumnName("template")
                .HasMaxLength(100)
                .HasDefaultValue("default");

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(PageStatus.Draft);

            builder.Property(a => a.IsPublic)
                .HasColumnName("is_public")
                .HasDefaultValue(true);

            builder.Property(a => a.RequireLogin)
                .HasColumnName("require_login")
                .HasDefaultValue(false);

            builder.Property(a => a.MetaTitle)
                .HasColumnName("meta_title")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.MetaDescription)
                .HasColumnName("meta_description")
                .IsRequired(false);

            builder.Property(a => a.MetaKeywords)
                .HasColumnName("meta_keywords")
                .IsRequired(false);

            builder.Property(a => a.ShowInFooter)
                .HasColumnName("show_in_footer")
                .HasDefaultValue(false);

            builder.Property(a => a.FooterOrder)
                .HasColumnName("footer_order")
                .IsRequired(false);

            builder.Property(a => a.AuthorId)
                .HasColumnName("author_id")
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired(false);

            builder.Property(a => a.PublishedAt)
               .HasColumnName("published_at")
               .IsRequired(false);

            // FK
            builder.HasOne<AdminUser>()
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(a => a.PageSlug)
                .HasFilter("status = 1")
                .HasDatabaseName("idx_pages_slug");

            builder.HasIndex(a => a.Status)
                .HasDatabaseName("idx_pages_status");

            builder.HasIndex(a => new { a.ShowInFooter, a.FooterOrder })
                .HasFilter("show_in_footer = TRUE AND status = 1")
                .HasDatabaseName("idx_pages_footer");
        }
    }
}
