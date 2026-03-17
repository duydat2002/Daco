namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IUserAddressRepository
    {
        Task<IReadOnlyList<UserAddress>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(UserAddress address, CancellationToken cancellationToken = default);
        Task<UserAddress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
