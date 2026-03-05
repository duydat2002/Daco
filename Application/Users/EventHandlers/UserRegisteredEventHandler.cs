using System.Numerics;

namespace Daco.Application.Users.EventHandlers
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly IVerificationTokenRepository _verificationTokenRepository;
        private readonly ILogger<UserRegisteredEventHandler> _logger;

        public UserRegisteredEventHandler(
            IEmailService emailService,
            ISmsService smsService,
            IVerificationTokenRepository verificationTokenRepository,
            ILogger<UserRegisteredEventHandler> logger)
        {
            _emailService = emailService;
            _smsService = smsService;
            _verificationTokenRepository = verificationTokenRepository;
            _logger = logger;
        }

        public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling UserRegisteredEvent for User {notification.UserId} with provider {notification.ProviderType}");

            try
            {
                var verificationToken = await _verificationTokenRepository.GenerateTokenAsync(
                    notification.UserId,
                    notification.ProviderType == ProviderTypes.Email ? 
                        VerificationTokenType.EmailVerification : 
                        VerificationTokenType.PhoneVerification,
                    cancellationToken);

                _logger.LogDebug($"Generated verification token: {verificationToken}");

                if (notification.ProviderType == ProviderTypes.Email)
                {
                    //await _emailService.SendOtpAsync(notification.Identifier, verificationToken, cancellationToken);
                }
                else if (notification.ProviderType == ProviderTypes.Phone)
                {
                    await _smsService.SendAsync(notification.Identifier, verificationToken, cancellationToken);
                }

                _logger.LogInformation($"User {notification.UserId} registered via {notification.ProviderType}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling UserRegisteredEvent for User {notification.UserId}");
            }
        }
    }
}
