namespace Daco.Application.Common.Interfaces.Repositories
{
    public interface IVerificationTokenRepository
    {
        Task<string> GenerateTokenAsync(
            Guid userId,
            string tokenType,
            CancellationToken cancellationToken = default);

        Task<bool> ValidateTokenAsync(
            Guid userId,
            string token,
            CancellationToken cancellationToken = default);

        Task<VerificationToken?> GetLatestAsync(
            Guid userId,
            string tokenType,
            CancellationToken cancellationToken = default);
    }
}
