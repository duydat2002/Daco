namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface ILoginSessionRepository
    {
        Task AddAsync(LoginSession session, CancellationToken cancellationToken = default);
    }
}
