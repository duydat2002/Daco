namespace Daco.Infrastructure.Persistence.Repositories.Brands
{
    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BrandRepository> _logger;

        public BrandRepository(
            AppDbContext context,
            ILogger<BrandRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(Brand brand, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, brand,
                async () => await _context.Set<Brand>().AddAsync(brand, cancellationToken));
        }

        public async Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.Set<Brand>()
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken));
        }

        public async Task<Brand?> GetByIdWithCategoriesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.Set<Brand>()
                    .Include(b => b.BrandCategories)
                    .FirstOrDefaultAsync(b => b.Id == id, cancellationToken));
        }

        public async Task<bool> NameExistsAsync(
            string name,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { name, excludeId },
                () => _context.Set<Brand>()
                    .AnyAsync(b => b.BrandName == name
                                && (excludeId == null || b.Id != excludeId),
                              cancellationToken));
        }

        public async Task<bool> SlugExistsAsync(
            string slug,
            Guid? excludeId = null,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { slug, excludeId },
                () => _context.Set<Brand>()
                    .AnyAsync(b => b.BrandSlug == slug
                                && (excludeId == null || b.Id != excludeId),
                              cancellationToken));
        }

        public async Task<bool> HasProductsAsync(
            Guid brandId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { brandId },
                () => _context.Set<Product>()
                    .AnyAsync(p => p.BrandId == brandId
                                && p.DeletedAt == null,
                              cancellationToken));
        }

        public async Task<PagedResult<BrandListItemDTO>> GetBrandsAsync(
            GetBrandsQuery query,
            CancellationToken cancellationToken = default)
        {
            var q = _context.Set<Brand>().AsQueryable();

            if (!string.IsNullOrEmpty(query.Search))
            {
                var search = query.Search.ToLower();
                q = q.Where(b => b.BrandName.ToLower().Contains(search));
            }

            if (query.IsActive.HasValue)
                q = q.Where(b => b.IsActive == query.IsActive.Value);

            var total = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderBy(b => b.BrandName)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(b => new BrandListItemDTO
                {
                    Id = b.Id,
                    BrandName = b.BrandName,
                    BrandSlug = b.BrandSlug,
                    Description = b.Description,
                    LogoUrl = b.LogoUrl,
                    IsActive = b.IsActive,
                    CreatedAt = b.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<BrandListItemDTO>
            {
                Items = items,
                Total = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public void Delete(Brand brand)
        {
            _context.Set<Brand>().Remove(brand);
        }
    }
}
