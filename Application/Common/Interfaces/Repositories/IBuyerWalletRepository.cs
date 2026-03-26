namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IBuyerWalletRepository
    {
        Task<UserWalletSummaryDTO?> GetSummaryByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);

        Task<PagedResult<UserTransactionListItemDTO>> GetTransactionsAsync(
            GetUserTransactionsQuery query,
            CancellationToken cancellationToken = default);
    }
}
