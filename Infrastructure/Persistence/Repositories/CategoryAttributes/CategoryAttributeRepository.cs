namespace Daco.Infrastructure.Persistence.Repositories.CategoryAttributes
{
    public class CategoryAttributeRepository : ICategoryAttributeRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryAttributeRepository> _logger;

        public CategoryAttributeRepository(
            AppDbContext context,
            ILogger<CategoryAttributeRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(
            CategoryAttribute attribute,
            CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, attribute,
                async () => await _context.CategoryAttributes
                    .AddAsync(attribute, cancellationToken));
        }

        public async Task<CategoryAttribute?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.CategoryAttributes
                    .FirstOrDefaultAsync(a => a.Id == id, cancellationToken));
        }

        public async Task<CategoryAttribute?> GetByIdWithValuesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.CategoryAttributes
                    .Include(a => a.CategoryAttributeValues)
                    .FirstOrDefaultAsync(a => a.Id == id, cancellationToken));
        }

        public async Task<IReadOnlyList<CategoryAttribute>> GetByCategoryIdAsync(
            Guid categoryId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { categoryId },
                () => _context.CategoryAttributes
                    .Include(a => a.CategoryAttributeValues.Where(v => v.IsActive))
                    .Where(a => a.CategoryId == categoryId && a.IsActive)
                    .OrderBy(a => a.SortOrder)
                    .ToListAsync(cancellationToken));
        }

        public async Task<bool> SlugExistsInCategoryAsync(
            Guid categoryId,
            string slug,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { categoryId, slug, excludeId },
                () => _context.CategoryAttributes
                    .AnyAsync(a => a.CategoryId == categoryId
                                && a.AttributeSlug == slug
                                && (excludeId == null || a.Id != excludeId),
                              cancellationToken));
        }

        public void Delete(CategoryAttribute attribute)
        {
            _context.CategoryAttributes.Remove(attribute);
        }
    }
}
