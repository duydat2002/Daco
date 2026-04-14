namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ShopId)
                .HasColumnName("shop_id")
                .IsRequired();

            builder.Property(a => a.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.Property(a => a.ProductName)
                .HasColumnName("product_name")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(a => a.ProductSlug)
                .HasColumnName("product_slug")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasColumnName("description")
                .IsRequired(false);

            builder.Property(a => a.BrandId)
                .HasColumnName("brand_id")
                .IsRequired(false);

            builder.Property(a => a.BasePrice)
                .HasColumnName("base_price")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired(false);

            builder.Property(a => a.CompareAtPrice)
                .HasColumnName("compare_at_price")
                .HasColumnType("DECIMAL(15,2)")
                .IsRequired(false);

            builder.Property(a => a.StockQuantity)
                .HasColumnName("stock_quantity")
                .HasDefaultValue(0);

            builder.Property(a => a.Sku)
                .HasColumnName("sku")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.Gtin)
                .HasColumnName("gtin")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(ProductStatus.Draft);

            builder.Property(a => a.CategoryId)
                .HasColumnName("category_id")
                .IsRequired(false);

            builder.Property(a => a.HasVariants)
                .HasColumnName("has_variants")
                .HasDefaultValue(false);

            builder.Property(a => a.IsPreOrder)
                .HasColumnName("is_pre_order")
                .HasDefaultValue(false);

            builder.Property(a => a.PreOrderLeadTime)
                .HasColumnName("pre_order_lead_time")
                .IsRequired(false);

            builder.Property(a => a.Weight)
                .HasColumnName("weight")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired();

            builder.Property(a => a.Length)
                .HasColumnName("length")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.Width)
                .HasColumnName("width")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.Height)
                .HasColumnName("height")
                .HasColumnType("DECIMAL(10,2)")
                .IsRequired(false);

            builder.Property(a => a.EnabledShippingTypes)
                .HasColumnName("enabled_shipping_types")
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'[]'::jsonb");

            builder.Property(a => a.ViewCount)
                .HasColumnName("view_count")
                .HasDefaultValue(0);

            builder.Property(a => a.SoldCount)
                .HasColumnName("sold_count")
                .HasDefaultValue(0);

            builder.Property(a => a.WishlistCount)
                .HasColumnName("wishlist_count")
                .HasDefaultValue(0);

            builder.Property(a => a.RatingAverage)
                .HasColumnName("rating_average")
                .HasColumnType("DECIMAL(3,2)")
                .HasDefaultValue(0);

            builder.Property(a => a.ReviewCount)
                .HasColumnName("review_count")
                .HasDefaultValue(0);

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

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(a => a.PublishedAt)
                .HasColumnName("published_at")
                .IsRequired(false);

            builder.Property(a => a.DeletedAt)
                .HasColumnName("deleted_at")
                .IsRequired(false);

            // FK
            builder.HasOne<Shop>()
               .WithMany()
               .HasForeignKey(a => a.ShopId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Brand>()
               .WithMany()
               .HasForeignKey(a => a.BrandId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne<Category>()
               .WithMany()
               .HasForeignKey(a => a.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.ProductImages)
                .WithOne()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ProductImages)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_productImages");

            builder.HasMany(p => p.ProductVideos)
                .WithOne()
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ProductVideos)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_productVideos");

            builder.HasMany(p => p.ProductDynamicAttributes)
                .WithOne()
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ProductDynamicAttributes)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_productDynamicAttributes");

            builder.HasMany(p => p.ProductVariantGroups)
                .WithOne()
                .HasForeignKey(vg => vg.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ProductVariantGroups)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_productVariantGroups");

            builder.HasMany(p => p.ProductVariants)
                .WithOne()
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.ProductVariants)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_productVariants");

            // Indexes
            builder.HasIndex(a => a.ShopId)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_products_shop");

            builder.HasIndex(a => a.CategoryId)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_products_category");

            builder.HasIndex(a => a.Status)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_products_status");

            builder.HasIndex(a => a.ProductSlug)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_products_slug");

            builder.HasIndex(a => a.BrandId)
                .HasFilter("deleted_at IS NULL")
                .HasDatabaseName("idx_products_brand");
        }
    }
}
