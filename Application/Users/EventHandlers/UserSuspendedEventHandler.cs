namespace Daco.Application.Users.EventHandlers
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
            _logger.LogWarning(
                "Handling UserSuspendedEvent for User {UserId}, Reason: {Reason}",
                notification.UserId,
                notification.Reason);

            try
            {
                var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);
                if (user == null || user.Email == null)
                    return;

                var subject = "Account Suspended";
                var body = $@"
                    <h2>Your Account Has Been Suspended</h2>
                    <p><strong>Reason:</strong> {notification.Reason}</p>
                    <p>Your account has been temporarily suspended.</p>
                    <p>Please contact support if you believe this is a mistake.</p>
                ";

                await _emailService.SendAsync(user.Email.Value, subject, body, cancellationToken);

                _logger.LogInformation("Suspension notification sent to user {UserId}", notification.UserId);

                // await _sessionService.RevokeAllSessionsAsync(notification.UserId);

                _logger.LogWarning(
                    "User {UserId} suspended at {Timestamp}, Reason: {Reason}",
                    notification.UserId,
                    DateTime.UtcNow,
                    notification.Reason);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling UserSuspendedEvent for User {UserId}",
                    notification.UserId);
            }
        }
    }
}
