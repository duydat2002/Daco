namespace Daco.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ParentId)
                .HasColumnName("parent_id")
                .IsRequired(false);

            builder.Property(a => a.CategoryName)
                .HasColumnName("category_name")
                .HasMaxLength(255)  
                .IsRequired();

            builder.Property(a => a.CategorySlug)
                .HasColumnName("category_slug")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.Level)
                .HasColumnName("level")
                .IsRequired();

            builder.Property(a => a.Path)
                .HasColumnName("path")
                .IsRequired(false);

            builder.Property(a => a.IconUrl)
                .HasColumnName("icon_url")
                .IsRequired(false);

            builder.Property(a => a.ImageUrl)
                .HasColumnName("image_url")
                .IsRequired(false);

            builder.Property(a => a.SortOrder)
                .HasColumnName("sort_order")
                .HasDefaultValue(0);

            builder.Property(a => a.IsActive)
               .HasColumnName("is_active")
               .HasDefaultValue(true);

            builder.Property(a => a.IsLeaf)
               .HasColumnName("is_leaf")
               .HasDefaultValue(false);

            builder.Property(a => a.ProductCount)
               .HasColumnName("product_count")
               .HasDefaultValue(0);

            builder.Property(a => a.CreatedAt)
               .HasColumnName("created_at")
               .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
               .HasColumnName("updated_at")
               .IsRequired(false);

            // FK
            builder.HasOne<Category>()
                .WithMany()
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.CategoryAttributes)
                .WithOne()
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.CategoryAttributes)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_categoryAttributes");

            // Indexes 
            builder.HasIndex(a => a.ParentId)
                .HasDatabaseName("idx_categories_parent");

            builder.HasIndex(a => a.Level)
                .HasDatabaseName("idx_categories_level");

            builder.HasIndex(a => a.IsLeaf)
                .HasFilter("is_leaf = TRUE")
                .HasDatabaseName("idx_categories_leaf");

            builder.HasIndex(a => a.Path)
                .HasDatabaseName("idx_categories_path");
        }
    }
}
