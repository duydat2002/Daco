namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface ILoginSessionRepository
    {
        Task AddAsync(LoginSession session, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<LoginSession>> GetAllActiveByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default);
        Task<LoginSession?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken cancellationToken = default);
        Task<LoginSession?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);
    }
}
