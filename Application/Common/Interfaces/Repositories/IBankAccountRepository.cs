namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task AddAsync(BankAccount bankAccount, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<BankAccount>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
