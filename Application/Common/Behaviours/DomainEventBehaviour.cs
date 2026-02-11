namespace Daco.Application.Common.Behaviours
{
    public class DomainEventBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<DomainEventBehaviour<TRequest, TResponse>> _logger;
        private readonly IPublisher _publisher;
        private readonly IUnitOfWork _unitOfWork;

        public DomainEventBehaviour(
            ILogger<DomainEventBehaviour<TRequest, TResponse>> logger,
            IPublisher publisher,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _publisher = publisher;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var response = await next();

            if (IsSuccessResponse(response))
            {
                await PublishDomainEventsAsync(cancellationToken);
            }

            return response;
        }

        private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
        {
            var domainEvents = _unitOfWork.GetTrackedEntities()
                .Where(e => e.DomainEvents.Any())
                .SelectMany(e =>
                {
                    var events = e.DomainEvents.ToList();
                    e.ClearDomainEvents();
                    return events;
                })
                .ToList();

            if (!domainEvents.Any())  return; 

            _logger.LogInformation(
                "Publishing {Count} domain events",
                domainEvents.Count
            );

            foreach (var domainEvent in domainEvents)
            {
                _logger.LogDebug(
                    "Publishing domain event: {EventType} - {EventData}",
                    domainEvent.GetType().Name,
                    JsonSerializer.Serialize(domainEvent)
                );

                try
                {
                    await _publisher.Publish(domainEvent, cancellationToken);

                    _logger.LogInformation(
                        "Domain event published successfully: {EventType}",
                        domainEvent.GetType().Name
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error publishing domain event: {EventType} - {ErrorMessage}",
                        domainEvent.GetType().Name,
                        ex.Message
                    );

                    throw;
                }
            }
        }

        private static bool IsSuccessResponse(TResponse response)
        {
            var isSuccessProperty = response?.GetType().GetProperty("IsSuccess");
            if (isSuccessProperty != null)
            {
                return (bool)(isSuccessProperty.GetValue(response) ?? false);
            }

            return true;
        }
    }
}
