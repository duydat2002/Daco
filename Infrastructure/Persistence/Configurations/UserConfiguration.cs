namespace Daco.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            // Value Objects
            builder.OwnsOne(u => u.Username, ub =>
            {
                ub.Property(a => a.Value)
                  .HasColumnName("username")
                  .HasMaxLength(50)
                  .IsRequired();

                ub.HasIndex(a => a.Value)
                    .IsUnique()
                    .HasFilter("deleted_at IS NULL")
                    .HasDatabaseName("idx_users_username");
            });

            builder.OwnsOne(u => u.Email, eb =>
            {
                eb.Property(a => a.Value)
                  .HasColumnName("email")
                  .HasMaxLength(100)
                  .IsRequired(false);

                eb.HasIndex(a => a.Value)
                    .IsUnique()
                    .HasFilter("deleted_at IS NULL")
                    .HasDatabaseName("idx_users_email");
            });
            

            builder.OwnsOne(u => u.Phone, pb =>
            {
                pb.Property(a => a.Value)
                  .HasColumnName("phone")
                  .HasMaxLength(20)
                  .IsRequired(false);

                pb.HasIndex(a => a.Value)
                    .IsUnique()
                    .HasFilter("deleted_at IS NULL")
                    .HasDatabaseName("idx_users_phone");
            });

            //builder.Property(a => a.Username)
            //    .HasColumnName("username")
            //    .HasMaxLength(50)
            //    .IsRequired()
            //    .HasConversion(
            //        v => v.Value,
            //        v => Username.Create(v));

            //builder.Property(a => a.Email)
            //    .HasColumnName("email")
            //    .HasMaxLength(100)
            //    .IsRequired(false)
            //    .HasConversion(
            //        v => v == null ? null : v.Value,
            //        v => v == null ? null : Email.Create(v));

            //builder.Property(a => a.Phone)
            //    .HasColumnName("phone")
            //    .HasMaxLength(20)
            //    .IsRequired(false)
            //    .HasConversion(
            //        v => v == null ? null : v.Value,
            //        v => v == null ? null : PhoneNumber.Create(v));

            // Primitive Properties
            builder.Property(a => a.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(a => a.Avatar)
                .HasColumnName("avatar")
                .IsRequired(false);

            builder.Property(a => a.DateOfBirth)
                .HasColumnName("date_of_birth")
                .IsRequired(false);

            builder.Property(a => a.Gender)
                .HasColumnName("gender")
                .HasConversion<int>();

            builder.Property(a => a.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .HasDefaultValue(UserStatus.Pending);

            builder.Property(a => a.EmailVerified)
                .HasColumnName("email_verified")
                .HasDefaultValue(false);

            builder.Property(a => a.PhoneVerified)
                .HasColumnName("phone_verified")
                .HasDefaultValue(false);

            // Timestamps
            builder.Property(a => a.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("NOW()");

            builder.Property(a => a.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired(false);

            builder.Property(a => a.DeletedAt)
                .HasColumnName("deleted_at")
                .IsRequired(false);

            // Unique Indexes
            //builder.HasIndex(a => a.Username)
            //    .IsUnique()
            //    .HasFilter("deleted_at IS NULL")
            //    .HasDatabaseName("idx_users_username");

            //builder.HasIndex(a => a.Email)
            //    .IsUnique()
            //    .HasFilter("deleted_at IS NULL")
            //    .HasDatabaseName("idx_users_email");

            //builder.HasIndex(a => a.Phone)
            //    .IsUnique()
            //    .HasFilter("deleted_at IS NULL")
            //    .HasDatabaseName("idx_users_phone");

            builder.HasIndex(a => a.Status)
                .HasDatabaseName("idx_users_status");

            // Collections 
            builder.HasMany(a => a.AuthProviders)
                .WithOne()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.AuthProviders)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_authProviders");

            builder.HasMany(a => a.Addresses)
                .WithOne()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.Addresses)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_addresses");

            builder.HasMany(a => a.BankAccounts)
                .WithOne()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(a => a.BankAccounts)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasField("_bankAccounts");
        }
    }
}
