namespace Daco.Application.Users.EventHandlers
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
            _logger.LogInformation($"Handling EmailVerifiedEvent for User {notification.UserId}");

            try
            {
                var subject = "Welcome to ECommerce!";
                var body = $@"
                    <h2>Email Verified Successfully!</h2>
                    <p>Your email {notification.Email} has been verified.</p>
                    <p>You can now:</p>
                    <ul>
                        <li>Browse products</li>
                        <li>Add items to cart</li>
                        <li>Place orders</li>
                    </ul>
                    <p>Happy shopping!</p>
                ";

                await _emailService.SendAsync(notification.Email, subject, body, cancellationToken);

                _logger.LogInformation($"Welcome email sent to {notification.Email}");

                _logger.LogInformation($"User {notification.UserId} verified email successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling EmailVerifiedEvent for User {notification.UserId}");
            }
        }
    }
}
