namespace Daco.Application.Users.EventHandlers
{
    public class PasswordChangedEventHandler : INotificationHandler<PasswordChangedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<PasswordChangedEventHandler> _logger;

        public PasswordChangedEventHandler(
            IEmailService emailService,
            IUserRepository userRepository,
            ILogger<PasswordChangedEventHandler> logger)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling PasswordChangedEvent for User {notification.UserId}");

            try
            {
                var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);
                if (user?.Email == null) return;

                await _emailService.SendPasswordChangedAsync(user.Email.Value, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling PasswordChangedEvent for User {notification.UserId}");
            }
        }
    }
}
