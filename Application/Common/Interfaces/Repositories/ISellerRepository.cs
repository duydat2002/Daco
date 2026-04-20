
namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface ISellerRepository
    {
        Task<bool> IsSellerAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Seller?> GetByIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
        Task<Seller?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(Seller seller, CancellationToken cancellationToken = default);
        Task UpdateAsync(Seller seller, CancellationToken cancellationToken = default);

    }
}
