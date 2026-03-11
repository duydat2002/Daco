namespace Daco.Application.Auth.EventHandlers
{
    public class EmailVerifiedEventHandler : INotificationHandler<EmailVerifiedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailVerifiedEventHandler> _logger;

        public EmailVerifiedEventHandler(
            IEmailService emailService,
            ILogger<EmailVerifiedEventHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(EmailVerifiedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling EmailVerifiedEvent for user {UserId}", notification.UserId);
            try
            {
                await _emailService.SendWelcomeAsync(notification.Email, notification.Email, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling EmailVerifiedEvent for user {UserId}", notification.UserId);
            }
        }
    }
}
