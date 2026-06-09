namespace Daco.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly List<AggregateRoot> _trackedEntities = new();
        private bool _disposed;

        public UnitOfWork(
            AppDbContext context,
            ILogger<UnitOfWork> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void TrackEntity(AggregateRoot entity)
        {
            if (!_trackedEntities.Contains(entity))
            {
                _trackedEntities.Add(entity);
                _logger.LogDebug("Tracking entity {EntityType} with Id {Id}", entity.GetType().Name, entity.Id);
            }
        }

        public IEnumerable<AggregateRoot> GetTrackedEntities()
        {
            return _trackedEntities.AsReadOnly();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Beginning transaction");
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var count = await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Saved {Count} changes", count);
                return count;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    var keyValues = entry.Properties
                        .Where(p => p.Metadata.IsPrimaryKey())
                        .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);

                    _logger.LogError(
                        "Concurrency exception. Entity={Entity}, State={State}, Keys={@Keys}",
                        entry.Metadata.ClrType.Name,
                        entry.State,
                        keyValues);
                }

                throw;
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
            _logger.LogInformation("Transaction committed");
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
            _trackedEntities.Clear();
            _logger.LogWarning("Transaction rolled back");
        }

        public void Dispose()
        {
            if(_disposed) return;
            _trackedEntities.Clear();
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
