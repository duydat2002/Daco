namespace Daco.Infrastructure.Persistence.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.ToTable("wishlists");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(a => a.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.UserId, a.ProductId })
                .IsUnique();

            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("idx_wishlists_user");

            builder.HasIndex(a => a.ProductId)
                .HasDatabaseName("idx_wishlists_product");
        }
    }
}
