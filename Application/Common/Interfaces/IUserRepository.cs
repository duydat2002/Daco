namespace Daco.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync1(Guid id, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<User?> FindByIdentifierAsync(string identifier, CancellationToken cancellationToken = default);
        Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<User?> FindByPhoneAsync(string phone, CancellationToken cancellationToken = default);
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<bool> CheckUserAuthProvider(Guid userId, string providerType, CancellationToken cancellationToken = default);
        Task AddAuthProviderAsync(Guid userId, AuthProvider provider, CancellationToken cancellationToken);
    }
}
