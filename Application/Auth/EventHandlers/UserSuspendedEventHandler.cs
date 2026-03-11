namespace Daco.Application.Auth.EventHandlers
{
    public class UserSuspendedEventHandler : INotificationHandler<UserSuspendedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserSuspendedEventHandler> _logger;

        public UserSuspendedEventHandler(
            IEmailService emailService,
            IUserRepository userRepository,
            ILogger<UserSuspendedEventHandler> logger)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(UserSuspendedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning($"Handling UserSuspendedEvent for User {notification.UserId}, Reason: {notification.Reason}");

            try
            {
                var user = await _userRepository.GetProfileAsync(notification.UserId, cancellationToken);
                if (user?.Email == null) return;

                await _emailService.SendAccountSuspendedAsync(user.Email.Value, notification.Reason, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling UserSuspendedEvent for User {notification.UserId}");
            }
        }
    }
}
