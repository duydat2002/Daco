namespace Daco.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbSession _session;
        private readonly IDomainEventDispatcher _eventDispatcher;
        private readonly ILogger<UnitOfWork> _logger;
        private readonly List<AggregateRoot> _trackedEntities = new();
        private bool _disposed;

        public UnitOfWork(
            IDbSession session,
            IDomainEventDispatcher eventDispatcher,
            ILogger<UnitOfWork> logger)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Beginning transaction");
            await _session.BeginTransactionAsync(cancellationToken);
        }

        public void TrackEntity(AggregateRoot entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (!_trackedEntities.Contains(entity))
            {
                _trackedEntities.Add(entity);
                _logger.LogDebug($"Tracking entity {entity.GetType().Name} with Id {entity.Id}");
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Saving changes with {_trackedEntities.Count} tracked entities");

            var domainEvents = _trackedEntities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            _logger.LogDebug($"Collected {domainEvents.Count} domain events");

            foreach (var entity in _trackedEntities)
            {
                entity.ClearDomainEvents();
            }

            await _session.CommitAsync(cancellationToken);
            _logger.LogInformation("Transaction committed successfully");

            foreach (var domainEvent in domainEvents)
            {
                _logger.LogDebug($"Dispatching domain event {domainEvent.GetType().Name}");

                await _eventDispatcher.DispatchAsync(domainEvent, cancellationToken);
            }

            _logger.LogInformation($"Dispatched {domainEvents.Count} domain events");

            var count = _trackedEntities.Count;
            _trackedEntities.Clear();

            return count;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogWarning("Rolling back transaction");

            await _session.RollbackAsync(cancellationToken);
            _trackedEntities.Clear();

            _logger.LogInformation("Transaction rolled back");
        }

        public IEnumerable<AggregateRoot> GetTrackedEntities()
        {
            return _trackedEntities.AsReadOnly();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _session?.Dispose();
                _trackedEntities.Clear();
            }

            _disposed = true;
        }
    }
}
