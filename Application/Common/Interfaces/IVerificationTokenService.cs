namespace Daco.Application.Common.Interfaces
{
    public interface IVerificationTokenService
    {
        Task<string> GenerateTokenAsync(Guid userId, string tokenType, CancellationToken cancellationToken = default);
        Task<bool> ValidateTokenAsync(Guid userId, string token, CancellationToken cancellationToken = default);
    }
}
