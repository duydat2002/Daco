namespace Daco.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> FindByPhoneAsync(string phone, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    }
}
