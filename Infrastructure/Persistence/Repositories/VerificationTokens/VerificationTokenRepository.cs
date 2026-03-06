namespace Daco.Infrastructure.Persistence.Repositories.VerificationTokens
{
    public class VerificationTokenRepository : IVerificationTokenRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<VerificationTokenRepository> _logger;

        public VerificationTokenRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<VerificationTokenRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task<string> GenerateTokenAsync(
            Guid userId,
            string tokenType,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Generating verification token for user {UserId}, type {TokenType}", userId, tokenType);

            var oldTokens = await _context.VerificationTokens
                .Where(t => t.UserId == userId
                         && t.TokenType == tokenType
                         && t.Status == VerificationStatus.Pending)
                .ToListAsync(cancellationToken);

            foreach (var old in oldTokens)
                old.Invalidate();

            var verificationToken = VerificationToken.Create(
                userId: userId,
                tokenType: tokenType,
                expirationMinutes: 15,
                maxAttempts: 5);

            await _context.VerificationTokens.AddAsync(verificationToken, cancellationToken);

            _logger.LogInformation("Verification token created for user {UserId}", userId);

            return verificationToken.Token;
        }

        public async Task<bool> ValidateTokenAsync(
            Guid userId,
            string token,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Validating token for user {UserId}", userId);

            var verificationToken = await _context.VerificationTokens
                .Where(t => t.UserId == userId && t.Status == VerificationStatus.Pending)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (verificationToken is null)
                return false;

            var isValid = verificationToken.Verify(token);

            await _context.SaveChangesAsync(cancellationToken);

            return isValid;
        }

        public async Task<VerificationToken?> GetLatestAsync(
            Guid userId, 
            string tokenType, 
            CancellationToken cancellationToken = default)
        {
            return await _context.VerificationTokens
                .Where(t => t.UserId == userId && t.TokenType == tokenType)
                .OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region Procedure
        #endregion
    }
}
