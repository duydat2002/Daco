namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ReviewReactionConfiguration : IEntityTypeConfiguration<ReviewReaction>
    {
        public void Configure(EntityTypeBuilder<ReviewReaction> builder)
        {
            builder.ToTable("review_reactions");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.ReviewId)
                .HasColumnName("review_id")
                .IsRequired();

            builder.Property(a => a.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(a => a.IsHelpful)
                .HasColumnName("is_helpful")
                .IsRequired();

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<User>()
               .WithMany()
               .HasForeignKey(a => a.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(a => new { a.ReviewId, a.UserId })
                .IsUnique();

            builder.HasIndex(a => a.ReviewId)
                .HasDatabaseName("idx_review_reactions_review");
        }
    }
}
