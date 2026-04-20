namespace Daco.Infrastructure.Persistence.Repositories.Withdrawals
{
    public class WithdrawalRepository : IWithdrawalRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<WithdrawalRepository> _logger;

        public WithdrawalRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<WithdrawalRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(
            SellerWithdrawalRequest withdrawal,
            CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, withdrawal,
                async () => await _context.Set<SellerWithdrawalRequest>()
                    .AddAsync(withdrawal, cancellationToken));
        }

        public async Task<SellerWithdrawalRequest?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.Set<SellerWithdrawalRequest>()
                    .FirstOrDefaultAsync(w => w.Id == id, cancellationToken));
        }

        public async Task<IReadOnlyList<SellerWithdrawalRequest>> GetBySellerIdAsync(
            Guid sellerId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { sellerId },
                () => _context.Set<SellerWithdrawalRequest>()
                    .Where(w => w.SellerId == sellerId)
                    .OrderByDescending(w => w.CreatedAt)
                    .ToListAsync(cancellationToken));
        }

        public async Task<decimal> GetTotalPendingAmountBySellerAsync(
            Guid sellerId,
            CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { sellerId },
                () => _context.Set<SellerWithdrawalRequest>()
                    .Where(w => w.SellerId == sellerId
                             && (w.Status == WithdrawalStatus.Pending
                              || w.Status == WithdrawalStatus.Approved
                              || w.Status == WithdrawalStatus.Processing))
                    .SumAsync(w => (decimal?)w.Amount, cancellationToken)
                    .ContinueWith(t => t.Result ?? 0m));
        }

        public async Task<PagedResult<WithdrawalListItemDTO>> GetWithdrawalsAsync(
            GetWithdrawalsQuery query,
            CancellationToken cancellationToken = default)
        {
            var q = _context.Set<SellerWithdrawalRequest>()
                .Join(_context.Sellers,
                    w => w.SellerId,
                    s => s.Id,
                    (w, s) => new { Withdrawal = w, Seller = s })
                .Join(_context.Users,
                    x => x.Seller.UserId,
                    u => u.Id,
                    (x, u) => new { x.Withdrawal, x.Seller, User = u })
                .AsQueryable();

            if (!string.IsNullOrEmpty(query.Status) &&
                Enum.TryParse<WithdrawalStatus>(query.Status, true, out var status))
            {
                q = q.Where(x => x.Withdrawal.Status == status);
            }

            if (query.SellerId.HasValue)
            {
                q = q.Where(x => x.Withdrawal.SellerId == query.SellerId.Value);
            }

            if (query.FromDate.HasValue)
            {
                q = q.Where(x => x.Withdrawal.CreatedAt >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                q = q.Where(x => x.Withdrawal.CreatedAt <= query.ToDate.Value);
            }

            var total = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderByDescending(x => x.Withdrawal.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new WithdrawalListItemDTO
                {
                    Id = x.Withdrawal.Id,
                    SellerId = x.Withdrawal.SellerId,
                    SellerUsername = x.User.Username.Value,
                    SellerEmail = x.User.Email.Value,
                    Amount = x.Withdrawal.Amount,
                    Fee = x.Withdrawal.Fee,
                    NetAmount = x.Withdrawal.NetAmount,
                    BankName = x.Withdrawal.BankName,
                    BankAccountNumber = x.Withdrawal.BankAccountNumber,
                    BankAccountName = x.Withdrawal.BankAccountName,
                    Status = x.Withdrawal.Status.ToString().ToLower(),
                    TransactionCode = x.Withdrawal.TransactionCode,
                    SellerNote = x.Withdrawal.SellerNote,
                    AdminNote = x.Withdrawal.AdminNote,
                    ApprovedAt = x.Withdrawal.ApprovedAt,
                    CompletedAt = x.Withdrawal.CompletedAt,
                    CreatedAt = x.Withdrawal.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<WithdrawalListItemDTO>
            {
                Items = items,
                Total = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }
        #endregion
    }
}
