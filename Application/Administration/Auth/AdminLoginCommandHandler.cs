namespace Daco.Application.Administration.Auth
{
    public class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminUserRepository _adminRepository;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AdminLoginCommandHandler> _logger;

        public AdminLoginCommandHandler(
            IUserRepository userRepository,
            IAdminUserRepository adminRepository,
            IVerificationTokenRepository tokenRepository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IEmailService emailService,
            ILogger<AdminLoginCommandHandler> logger)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _tokenRepository = tokenRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin login attempt: {Identifier}", request.Identifier);

            var normalized = request.Identifier.Trim().ToLowerInvariant();
            var user = await _userRepository.FindByIdentifierAsync(normalized, cancellationToken);

            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            var adminUser = await _adminRepository.GetByUserIdAsync(user.Id, cancellationToken);
            if (adminUser is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            if (adminUser.Status == AdminStatus.Suspended)
                return ResponseDTO.Failure(ErrorCodes.Auth.AccountSuspended, "Your account has been suspended");

            if (adminUser.Status == AdminStatus.Inactive)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            var provider = user.AuthProviders
                .FirstOrDefault(p => (p.ProviderType == ProviderTypes.Email
                                   || p.ProviderType == ProviderTypes.Phone)
                                  && p.DeletedAt == null);

            if (provider is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            if (!_passwordHasher.VerifyPassword(request.Password, provider.PasswordHash!))
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            var otp = await _tokenRepository.GenerateTokenAsync(
                user.Id,
                VerificationTokenTypes.AdminTwoFactor,
                cancellationToken);

            var tempToken = _jwtService.GenerateTempToken(user.Id);

            if (user.Email is not null)
                await _emailService.SendAdminOtpAsync(
                    user.Email.Value,
                    otp,
                    cancellationToken);

            _logger.LogInformation("Admin OTP sent for user {UserId}", user.Id);

            return ResponseDTO.Success(new
            {
                TempToken = tempToken,
                ExpiredIn = 300 
            }, "OTP has been sent to your email");
        }
    }
}
