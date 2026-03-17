namespace Daco.Infrastructure.Persistence.Repositories.UserAddresses
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<UserAddressRepository> _logger;

        public UserAddressRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<UserAddressRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(UserAddress address, CancellationToken cancellationToken = default)
        {
            await _context.UserAddresses.AddAsync(address, cancellationToken);
        }

        public async Task<IReadOnlyList<UserAddress>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserAddresses
                .Where(a => a.UserId == userId && a.DeletedAt == null)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<UserAddress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.UserAddresses
                .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null, cancellationToken);
        }
        #endregion
    }
}
