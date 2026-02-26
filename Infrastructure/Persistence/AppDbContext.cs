namespace Daco.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            //modelBuilder.HasPostgresEnum<UserGender>();
            //modelBuilder.HasPostgresEnum<UserStatus>();
            //modelBuilder.HasPostgresEnum<VerificationStatus>();
            //modelBuilder.HasPostgresEnum<VerificationTokenType>();
        }
    }
}
