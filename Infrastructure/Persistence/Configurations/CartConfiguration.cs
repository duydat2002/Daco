namespace Daco.Infrastructure.Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("carts");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.HasIndex(a => a.UserId)
                .IsUnique();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            // FK
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.CartItems)
                .WithOne()
                .HasForeignKey(a => a.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.CartItems)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_cartItems");

            // Indexes
            builder.HasIndex(a => a.UserId)
               .HasDatabaseName("idx_carts_user");
        }
    }
}
