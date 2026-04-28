
namespace Daco.Infrastructure.Persistence.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(
            AppDbContext context,
            ILogger<CategoryRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, category,
                async () => await _context.Categories.AddAsync(category, cancellationToken));
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.Categories
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken));
        }

        public async Task<bool> ExistsBySlugAsync(
            string slug,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { slug, excludeId },
                () => _context.Categories
                    .AnyAsync(c => c.CategorySlug == slug
                                && (excludeId == null || c.Id != excludeId),
                              cancellationToken));
        }

        public async Task<IReadOnlyList<Category>> GetChildrenAsync(
            Guid parentId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { parentId },
                () => _context.Categories
                    .Where(c => c.ParentId == parentId && c.IsActive)
                    .OrderBy(c => c.SortOrder)
                    .ToListAsync(cancellationToken));
        }

        public async Task<IReadOnlyList<Category>> GetLeafCategoriesAsync(
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, null,
                () => _context.Categories
                    .Where(c => c.IsLeaf && c.IsActive)
                    .OrderBy(c => c.SortOrder)
                    .ToListAsync(cancellationToken));
        }

        public async Task<IReadOnlyList<Category>> GetByIdsAsync(
            List<Guid> ids,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { ids },
                () => _context.Set<Category>()
                    .Where(c => ids.Contains(c.Id))
                    .ToListAsync(cancellationToken));
        }

        public Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> GetByOrderCodeAsync(string categoryId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Category order, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<bool> HasChildrenAsync(
            Guid categoryId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { categoryId },
                () => _context.Categories
                    .AnyAsync(c => c.ParentId == categoryId, cancellationToken));
        }

        public async Task<bool> HasProductsAsync(
            Guid categoryId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { categoryId },
                () => _context.Set<Product>()
                    .AnyAsync(p => p.CategoryId == categoryId
                                && p.DeletedAt == null, cancellationToken));
        }
    }
}
