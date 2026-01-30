namespace Daco.Domain.Users.Entities
{
    /// <summary>
    /// Token xác thực email/phone/reset password
    /// </summary>
    public class VerificationToken : Entity
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; }
        public VerificationTokenType TokenType { get; private set; } 
        public VerificationStatus Status { get; private set; }
        public int Attempts { get; private set; }
        public int MaxAttempts { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime? VerifiedAt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private VerificationToken() { } // For EF Core

        public static VerificationToken Create(
            Guid userId,
            VerificationTokenType tokenType,
            int expirationMinutes = 15,
            int maxAttempts = 5)
        {
            return new VerificationToken
            {
                UserId = userId,
                Token = GenerateToken(),
                TokenType = tokenType,
                Status = VerificationStatus.Pending,
                Attempts = 0,
                MaxAttempts = maxAttempts,
                ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
                CreatedAt = DateTime.UtcNow
            };
        }

        private static string GenerateToken()
        {
            // Generate 6-digit OTP
            return Random.Shared.Next(100000, 999999).ToString();
        }

        public bool Verify(string inputToken)
        {
            if (Status != VerificationStatus.Pending)
                return false;

            if (DateTime.UtcNow > ExpiresAt)
            {
                Status = VerificationStatus.Expired;
                return false;
            }

            Attempts++;

            if (Attempts > MaxAttempts)
            {
                Status = VerificationStatus.Failed;
                return false;
            }

            if (Token != inputToken)
                return false;

            Status = VerificationStatus.Verified;
            VerifiedAt = DateTime.UtcNow;
            return true;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }

        public bool CanRetry()
        {
            return Attempts < MaxAttempts && Status == VerificationStatus.Pending && !IsExpired();
        }
    }
}
