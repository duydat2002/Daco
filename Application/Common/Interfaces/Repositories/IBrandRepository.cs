namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IBrandRepository 
    {
        Task AddAsync(Brand brand, CancellationToken cancellationToken = default);
        Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Brand?> GetByIdWithCategoriesAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> NameExistsAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> HasProductsAsync(Guid brandId, CancellationToken cancellationToken = default);
        Task<PagedResult<BrandListItemDTO>> GetBrandsAsync(GetBrandsQuery query, CancellationToken cancellationToken = default);
        void Delete(Brand brand);
    }
}
