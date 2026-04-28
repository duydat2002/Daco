
namespace Daco.Infrastructure.Persistence.Repositories.Shops
{
    public class ShopRepository : IShopRepository
    {
        public Task AddAsync(Shop shop, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsBySlugAsync(string slug, Guid? excludeShopId = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Shop?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Shop?> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Shop shop, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
