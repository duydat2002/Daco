namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IShopRepository
    {
        Task<Shop?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Shop?> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
        Task<bool> ExistsBySlugAsync(string slug, Guid? excludeShopId = null, CancellationToken cancellationToken = default);
        Task AddAsync(Shop shop, CancellationToken cancellationToken = default);
        Task UpdateAsync(Shop shop, CancellationToken cancellationToken = default);

    }
}
