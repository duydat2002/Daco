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
            _logger.LogInformation(
                "Handling PasswordChangedEvent for User {UserId}",
                notification.UserId);

            try
            {
                var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);
                if (user == null || user.Email == null)
                    return;

                var subject = "Password Changed - Security Alert";
                var body = $@"
                    <h2>Password Changed</h2>
                    <p>Your password was changed at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                    <p>If you did not make this change, please contact support immediately.</p>
                ";

                await _emailService.SendAsync(user.Email.Value, subject, body, cancellationToken);

                _logger.LogInformation("Password change alert sent to user {UserId}", notification.UserId);

                _logger.LogWarning(
                    "User {UserId} changed password at {Timestamp}",
                    notification.UserId,
                    DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling PasswordChangedEvent for User {UserId}",
                    notification.UserId);
            }
        }
    }
}
