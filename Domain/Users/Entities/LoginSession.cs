namespace Daco.Domain.Users.Entities
{
    public class LoginSession : Entity
    {
        public Guid UserId { get; private set; }
        public ProviderType LoginProvider { get; private set; }

        // Session info
        public string Token { get; private set; }              // JWT token hash
        public string? RefreshToken { get; private set; }

        // Device info
        public string? UserAgent { get; private set; }
        public string? DeviceType { get; private set; }        // mobile, tablet, desktop
        public string IpAddress { get; private set; }

        // Status
        public bool IsActive { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? RevokeReason { get; private set; }      // user_logout, admin_action, suspicious

        // Timestamps
        public DateTime LoginAt { get; private set; }
        public DateTime LastActivityAt { get; private set; }
        public DateTime ExpiresAt { get; private set; }

        private LoginSession() { } // For EF Core

        public static LoginSession Create(
            Guid userId,
            ProviderType loginProvider,
            string token,
            string? refreshToken,
            string ipAddress,
            string? userAgent,
            string? deviceType,
            int expirationHours = 24)
        {
            Guard.AgainstNullOrEmpty(token, nameof(token));
            Guard.AgainstNullOrEmpty(ipAddress, nameof(ipAddress));

            return new LoginSession
            {
                UserId = userId,
                LoginProvider = loginProvider,
                Token = token,
                RefreshToken = refreshToken,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                DeviceType = deviceType,
                IsActive = true,
                LoginAt = DateTime.UtcNow,
                LastActivityAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
            };
        }

        public void UpdateActivity()
        {
            LastActivityAt = DateTime.UtcNow;
        }

        public void Revoke(string reason)
        {
            IsActive = false;
            RevokedAt = DateTime.UtcNow;
            RevokeReason = reason;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }
    }
}
