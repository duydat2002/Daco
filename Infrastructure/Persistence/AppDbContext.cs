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
        public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
        public DbSet<AdminRoleAssignment> AdminRoleAssignments => Set<AdminRoleAssignment>();
        public DbSet<AdminCustomPermission> AdminCustomPermissions => Set<AdminCustomPermission>();
        public DbSet<AdminRole> AdminRoles => Set<AdminRole>();
        public DbSet<AdminPermission> AdminPermissions => Set<AdminPermission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Product> Products => Set<Product>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
    