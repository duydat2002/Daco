namespace Daco.Application.Users.EventHandlers
{
    public class AddressAddedEventHandler : INotificationHandler<AddressAddedEvent>
    {
        private readonly ILogger<AddressAddedEventHandler> _logger;

        public AddressAddedEventHandler(ILogger<AddressAddedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(AddressAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling AddressAddedEvent for User {UserId}, Address {AddressId}",
                notification.UserId,
                notification.AddressId);

            try
            {
                _logger.LogInformation(
                    "User {UserId} added new address {AddressId}",
                    notification.UserId,
                    notification.AddressId);

                // await _searchIndexService.UpdateUserAddressesAsync(notification.UserId);

                // await _recommendationService.UpdateUserLocationAsync(notification.UserId);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling AddressAddedEvent for User {UserId}",
                    notification.UserId);
            }
        }
    }
}
