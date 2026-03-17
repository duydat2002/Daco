namespace Daco.Infrastructure.Persistence.Repositories.UserBankAccounts
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<BankAccountRepository> _logger;

        public BankAccountRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<BankAccountRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(BankAccount bankAccount, CancellationToken cancellationToken = default)
        {
            await _context.BankAccounts.AddAsync(bankAccount, cancellationToken);
        }

        public async Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.BankAccounts
                .FirstOrDefaultAsync(a => a.Id == id && a.DeletedAt == null, cancellationToken);
        }

        public async Task<IReadOnlyList<BankAccount>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.BankAccounts
                .Where(b => b.UserId == userId && b.DeletedAt == null)
                .OrderByDescending(b => b.IsDefault)
                .ThenByDescending(b => b.CreatedAt)
                .ToListAsync(cancellationToken);
        }
        #endregion
    }
}
