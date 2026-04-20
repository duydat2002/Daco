namespace Daco.Infrastructure.Persistence.Repositories.Sellers
{
    public class SellerRepository : ISellerRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<SellerRepository> _logger;

        public SellerRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<SellerRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(Seller seller, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, seller,
                async () => await _context.Sellers.AddAsync(seller, cancellationToken));
        }

        public async Task<Seller?> GetByIdAsync(Guid sellerId, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { sellerId },
                () => _context.Sellers
                    .FirstOrDefaultAsync(s => s.Id == sellerId && s.DeletedAt == null,
                        cancellationToken));
        }

        public async Task<Seller?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { userId },
                () => _context.Sellers
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.DeletedAt == null,
                        cancellationToken));
        }

        public async Task<bool> IsSellerAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { userId },
                () => _context.Sellers
                    .AnyAsync(s => s.UserId == userId
                        && s.Status == SellerStatus.Active
                        && s.DeletedAt == null,
                    cancellationToken));
        }

        public async Task UpdateAsync(Seller seller, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, seller,
                () =>
                {
                    _context.Sellers.Update(seller);
                    return Task.CompletedTask;
                });
        }
        #endregion
    }
}
