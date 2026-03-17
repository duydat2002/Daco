
namespace Daco.Application.Users.EventHandlers
{
    public class EmailChangeRequestedEventHandler : INotificationHandler<EmailChangeRequestedEvent>
    {
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailChangeRequestedEventHandler> _logger;

        public EmailChangeRequestedEventHandler(
            IVerificationTokenRepository tokenRepository,
            IEmailService emailService,
            ILogger<EmailChangeRequestedEventHandler> logger)
        {
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(EmailChangeRequestedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending OTP to new email {Email} for user {UserId}",
                notification.NewEmail, notification.UserId);
            try
            {
                var otp = await _tokenRepository.GenerateTokenAsync(
                    notification.UserId,
                    VerificationTokenTypes.EmailVerification,
                    cancellationToken);

                await _emailService.SendOtpAsync(notification.NewEmail, otp, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending OTP for email change, user {UserId}", notification.UserId);
            }
        }
    }
}
