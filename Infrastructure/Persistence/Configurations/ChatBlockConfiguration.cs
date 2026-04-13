namespace Daco.Infrastructure.Persistence.Configurations
{
    public class ChatBlockConfiguration : IEntityTypeConfiguration<ChatBlock>
    {
        public void Configure(EntityTypeBuilder<ChatBlock> builder)
        {
            builder.ToTable("chat_blocks");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            builder.Property(a => a.BlockerUserId)
                .HasColumnName("blocker_user_id")
                .IsRequired();

            builder.Property(a => a.BlockedUserId)
                .HasColumnName("blocked_user_id")
                .IsRequired();

            builder.Property(a => a.BlockType)
                .HasColumnName("block_type")
                .HasConversion<int>()
                .HasDefaultValue(ChatBlockType.Message);

            builder.Property(a => a.Reason)
                .HasColumnName("reason")
                .HasMaxLength(255)
                .IsRequired(false);

            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            // FK
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.BlockerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.BlockedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
