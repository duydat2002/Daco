namespace Daco.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<AuthProvider> AuthProviders => Set<AuthProvider>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
        public DbSet<LoginSession> LoginSessions => Set<LoginSession>();
        public DbSet<UserAddress> UserAddresses => Set<UserAddress>();
        public DbSet<VerificationToken> VerificationTokens => Set<VerificationToken>();
        public DbSet<Seller> Sellers => Set<Seller>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
    