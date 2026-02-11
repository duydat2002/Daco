using Daco.Domain.Users.Aggregates;
using Daco.Domain.Users.Constants;

namespace Daco.Domain.Users.Entities
{
    /// <summary>
    /// Auth Provider Entity - quản lý các phương thức đăng nhập
    /// </summary>
    public class AuthProvider : Entity
    {
        public Guid UserId { get; private set; }
        public string ProviderType { get; private set; }
        public string ProviderKey { get; private set; }

        // Password (only for email/phone providers)
        public string? PasswordHash { get; private set; }
        public DateTime? PasswordUpdatedAt { get; private set; }

        // Social provider specific data
        public string? ProviderUserId { get; private set; }
        public string? ProviderEmail { get; private set; }
        public string? ProviderName { get; private set; }
        public string? ProviderAvatar { get; private set; }

        // Tokens for OAuth refresh
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? TokenExpiresAt { get; private set; }

        // Verification
        public bool IsVerified { get; private set; }
        public DateTime? VerifiedAt { get; private set; }

        // Timestamps
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private AuthProvider() { }

        public static AuthProvider CreateEmailProvider(Guid userId, string email, string passwordHash)
        {
            Guard.AgainstNullOrEmpty(email, nameof(email));
            Guard.AgainstNullOrEmpty(passwordHash, nameof(passwordHash));

            return new AuthProvider
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProviderType = ProviderTypes.Email,
                ProviderKey = email.ToLowerInvariant(),
                PasswordHash = passwordHash,
                PasswordUpdatedAt = DateTime.UtcNow,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AuthProvider CreatePhoneProvider(Guid userId, string phone, string passwordHash)
        {
            Guard.AgainstNullOrEmpty(phone, nameof(phone));
            Guard.AgainstNullOrEmpty(passwordHash, nameof(passwordHash));

            return new AuthProvider
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProviderType = ProviderTypes.Phone,
                ProviderKey = phone,
                PasswordHash = passwordHash,
                PasswordUpdatedAt = DateTime.UtcNow,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static AuthProvider CreateSocialProvider(
            Guid userId,
            string providerType,
            string providerUserId,
            string? email,
            string? name,
            string? avatar,
            string? accessToken = null,
            string? refreshToken = null,
            DateTime? tokenExpiresAt = null
        )
        {
            Guard.Against(
                providerType != ProviderTypes.Google && providerType != ProviderTypes.Facebook,
                "Invalid social provider type");

            Guard.AgainstNullOrEmpty(providerUserId, nameof(providerUserId));

            return new AuthProvider
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ProviderType = providerType,
                ProviderKey = $"{providerType}:{providerUserId}",
                ProviderUserId = providerUserId,
                ProviderEmail = email,
                ProviderName = name,
                ProviderAvatar = avatar,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenExpiresAt = tokenExpiresAt,
                IsVerified = true, // Social logins are pre-verified
                VerifiedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
        }

        // Business logic
        public void UpdatePassword(string newPasswordHash)
        {
            Guard.AgainstNullOrEmpty(newPasswordHash, nameof(newPasswordHash));
            Guard.Against(
                ProviderType != ProviderTypes.Email && ProviderType != ProviderTypes.Phone,
                "Cannot update password for social providers");

            PasswordHash = newPasswordHash;
            PasswordUpdatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void MarkAsVerified()
        {
            IsVerified = true;
            VerifiedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateSocialTokens(string accessToken, string? refreshToken, DateTime expiresAt)
        {
            Guard.Against(
                ProviderType == ProviderTypes.Email || ProviderType == ProviderTypes.Phone,
                "Cannot update tokens for email/phone providers");

            AccessToken = accessToken;
            RefreshToken = refreshToken;
            TokenExpiresAt = expiresAt;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsTokenExpired()
        {
            return TokenExpiresAt.HasValue && TokenExpiresAt.Value < DateTime.UtcNow;
        }
    }
}
