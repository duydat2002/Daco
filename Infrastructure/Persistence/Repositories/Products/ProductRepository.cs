using Daco.Domain.Administration.Aggregates;

namespace Daco.Infrastructure.Persistence.Repositories.Products
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<ProductRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, product,
            () =>
            {
                _context.Products.Update(product);
                return Task.CompletedTask;
            });
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null, cancellationToken);
        }
        #endregion
    }
}
