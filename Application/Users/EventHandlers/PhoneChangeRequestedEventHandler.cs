namespace Daco.Application.Users.EventHandlers
{
    public class PhoneChangeRequestedEventHandler : INotificationHandler<PhoneChangeRequestedEvent>
    {
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly ISmsService _smsService;
        private readonly ILogger<PhoneChangeRequestedEventHandler> _logger;

        public PhoneChangeRequestedEventHandler(
            IVerificationTokenRepository tokenRepository,
            ISmsService smsService,
            ILogger<PhoneChangeRequestedEventHandler> logger)
        {
            _tokenRepository = tokenRepository;
            _smsService = smsService;
            _logger = logger;
        }

        public async Task Handle(PhoneChangeRequestedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending OTP to new phone {Phone} for user {UserId}",
                notification.NewPhone, notification.UserId);
            try
            {
                var otp = await _tokenRepository.GenerateTokenAsync(
                    notification.UserId,
                    VerificationTokenTypes.PhoneVerification,
                    cancellationToken);

                await _smsService.SendAsync(notification.NewPhone, otp, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending OTP for phone change, user {UserId}", notification.UserId);
            }
        }
    }
}
