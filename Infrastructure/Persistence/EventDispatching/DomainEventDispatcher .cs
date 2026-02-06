namespace Daco.Infrastructure.Persistence.EventDispatching
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(
            IMediator mediator,
            ILogger<DomainEventDispatcher> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DispatchAsync(
            DomainEvent domainEvent,
            CancellationToken cancellationToken = default)
        {
            if (domainEvent == null)
                throw new ArgumentNullException(nameof(domainEvent));

            _logger.LogInformation($"Dispatching domain event {domainEvent.GetType().Name} with ID {domainEvent.Id}");

            try
            {
                await _mediator.Publish(domainEvent, cancellationToken);

                _logger.LogInformation($"Successfully dispatched domain event {domainEvent.GetType().Name}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error dispatching domain event {domainEvent.GetType().Name}");
                throw;
            }
        }

        public async Task DispatchAsync(
            IEnumerable<DomainEvent> domainEvents,
            CancellationToken cancellationToken = default)
        {
            if (domainEvents == null)
                throw new ArgumentNullException(nameof(domainEvents));

            var events = domainEvents.ToList();

            _logger.LogInformation($"Dispatching {events.Count} domain events");

            foreach (var domainEvent in events)
            {
                await DispatchAsync(domainEvent, cancellationToken);
            }
        }
    }
}
