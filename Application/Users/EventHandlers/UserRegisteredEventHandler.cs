namespace Daco.Application.Users.EventHandlers
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly IVerificationTokenService _tokenService;
        private readonly ILogger<UserRegisteredEventHandler> _logger;

        public UserRegisteredEventHandler(
            IEmailService emailService,
            ISmsService smsService,
            IVerificationTokenService tokenService,
            ILogger<UserRegisteredEventHandler> logger)
        {
            _emailService = emailService;
            _smsService = smsService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling UserRegisteredEvent for User {notification.UserId} with provider {notification.ProviderType}");

            try
            {
                var verificationToken = await _tokenService.GenerateTokenAsync(
                    notification.UserId,
                    notification.ProviderType == ProviderType.Email ? "email_verification" : "phone_verification",
                    cancellationToken);

                _logger.LogDebug($"Generated verification token: {verificationToken}");

                if (notification.ProviderType == ProviderType.Email)
                {
                    await SendEmailVerification(notification.Identifier, verificationToken, cancellationToken);
                }
                else if (notification.ProviderType == ProviderType.Phone)
                {
                    await SendPhoneVerification(notification.Identifier, verificationToken, cancellationToken);
                }

                _logger.LogInformation($"User {notification.UserId} registered via {notification.ProviderType}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling UserRegisteredEvent for User {notification.UserId}");
            }
        }

        private async Task SendEmailVerification(string email, string token, CancellationToken cancellationToken)
        {
            var subject = "Verify your email";
            var body = $@"
                <h2>Welcome to ECommerce!</h2>
                <p>Please verify your email by entering this code:</p>
                <h1>{token}</h1>
                <p>This code will expire in 15 minutes.</p>
            ";

            await _emailService.SendAsync(email, subject, body, cancellationToken);

            _logger.LogInformation($"Verification email sent to {email}");
        }

        private async Task SendPhoneVerification(string phone, string token, CancellationToken cancellationToken)
        {
            var message = $"Your verification code is: {token}. Valid for 15 minutes.";

            await _smsService.SendAsync(phone, message, cancellationToken);

            _logger.LogInformation($"Verification SMS sent to {phone}");
        }
    }
}
