namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface ISellerWalletRepository
    {
        Task AddAsync(SellerWallet wallet, CancellationToken cancellationToken = default);
        Task<SellerWallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<SellerWallet?> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
    }
}
