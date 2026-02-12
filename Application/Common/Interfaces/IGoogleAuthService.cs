namespace Daco.Application.Common.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<GoogleUserInfo?> VerifyIdTokenAsync(string idToken);
    }

    public class GoogleUserInfo
    {
        public string Subject { get; init; } = default!;
        public string Email { get; init; } = default!;
        public bool EmailVerified { get; init; }
        public string Name { get; init; } = default!;
        public string Picture { get; init; } = default!;
    }
}
