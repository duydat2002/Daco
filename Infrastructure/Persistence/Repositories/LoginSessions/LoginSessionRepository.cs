namespace Daco.Infrastructure.Persistence.Repositories.LoginSessions
{
    public class LoginSessionRepository : ILoginSessionRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<LoginSessionRepository> _logger;

        public LoginSessionRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<LoginSessionRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(LoginSession session, CancellationToken cancellationToken = default)
        {
            await _context.LoginSessions.AddAsync(session, cancellationToken);
            _logger.LogDebug("LoginSession added for user {UserId}", session.UserId);
        }

        public async Task<LoginSession?> GetByTokenHashAsync(
            string tokenHash,
            CancellationToken cancellationToken = default)
        {
            return await _context.LoginSessions
                .Where(s => s.Token == tokenHash && s.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<LoginSession?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            return await _context.LoginSessions
                .Where(s => s.RefreshToken == refreshToken && s.IsActive)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<LoginSession>> GetAllActiveByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            return await _context.LoginSessions
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
