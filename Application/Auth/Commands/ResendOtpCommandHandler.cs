namespace Daco.Application.Auth.Commands
{
    public class ResendOtpCommandHandler : IRequestHandler<ResendOtpCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<ResendOtpCommandHandler> _logger;
        private readonly OtpSettings _otpSettings;

        public ResendOtpCommandHandler(
            IUserRepository userRepository,
            IVerificationTokenRepository tokenRepository,
            IEmailService emailService,
            IOptions<OtpSettings> settings,
            ILogger<ResendOtpCommandHandler> logger)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _logger = logger;
            _otpSettings = settings.Value;
        }

        public async Task<ResponseDTO> Handle(ResendOtpCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Resending OTP for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdWithProvidersAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            if (user.EmailVerified)
                return ResponseDTO.Failure(ErrorCodes.Auth.EmailNotVerified, "Email is already verified");

            var latest = await _tokenRepository.GetLatestAsync(
                request.UserId,
                VerificationTokenTypes.EmailVerification,
                cancellationToken);

            if (latest is not null)
            {
                var secondsSinceLast = (DateTime.UtcNow - latest.CreatedAt).TotalSeconds;
                if (secondsSinceLast < _otpSettings.ResendCooldownSeconds)
                {
                    var waitSeconds = (int)(_otpSettings.ResendCooldownSeconds - secondsSinceLast);
                    return ResponseDTO.Failure(
                        ErrorCodes.Auth.TooManyRequests,
                        $"Please wait {waitSeconds} seconds before requesting a new OTP");
                }
            }

            var otp = await _tokenRepository.GenerateTokenAsync(
                request.UserId,
                VerificationTokenTypes.EmailVerification,
                cancellationToken);

            await _emailService.SendOtpAsync(user.Email!.Value, otp);
            _logger.LogInformation("OTP resent for user {UserId}", request.UserId);

            return ResponseDTO.Success(null, $"OTP has been resent to {user.Email!.Value}");

        }
    }
}
