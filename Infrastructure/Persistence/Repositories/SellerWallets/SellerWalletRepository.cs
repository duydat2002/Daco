namespace Daco.Infrastructure.Persistence.Repositories.SellerWallets
{
    public class SellerWalletRepository : ISellerWalletRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<SellerWalletRepository> _logger;

        public SellerWalletRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<SellerWalletRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(
            SellerWallet wallet,
            CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, wallet,
                async () => await _context.Set<SellerWallet>()
                    .AddAsync(wallet, cancellationToken));
        }

        public async Task<SellerWallet?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.Set<SellerWallet>()
                    .FirstOrDefaultAsync(w => w.Id == id, cancellationToken));
        }

        public async Task<SellerWallet?> GetBySellerIdAsync(
            Guid sellerId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { sellerId },
                () => _context.Set<SellerWallet>()
                    .FirstOrDefaultAsync(w => w.SellerId == sellerId, cancellationToken));
        }
        #endregion
    }
}
