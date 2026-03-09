namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface ILoginSessionRepository
    {
        Task<LoginSession?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);
        Task AddAsync(LoginSession session, CancellationToken cancellationToken = default);
    }
}
