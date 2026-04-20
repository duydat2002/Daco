namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IWithdrawalRepository
    {
        Task AddAsync(SellerWithdrawalRequest withdrawal, CancellationToken cancellationToken = default);
        Task<SellerWithdrawalRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PagedResult<WithdrawalListItemDTO>> GetWithdrawalsAsync(GetWithdrawalsQuery query, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SellerWithdrawalRequest>> GetBySellerIdAsync(Guid sellerId, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalPendingAmountBySellerAsync(Guid sellerId, CancellationToken cancellationToken = default);
    }
}
