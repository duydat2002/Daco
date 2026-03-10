namespace Daco.Application.Users.Commands.Authentication
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly SessionSettings _sessionSettings;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IUserRepository userRepository,
            ILoginSessionRepository sessionRepository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService,
            IOptions<SessionSettings> sessionSettings,
            ILogger<LoginCommandHandler> logger)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _sessionSettings = sessionSettings.Value;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Login attempt for identifier: {Identifier}", request.Identifier);

            var user = await _userRepository.FindByIdentifierAsync(request.Identifier, cancellationToken);

            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            if (user.Status == UserStatus.Suspended)
                return ResponseDTO.Failure(ErrorCodes.Auth.AccountSuspended, "Your account has been suspended");

            if (user.Status == UserStatus.Banned)
                return ResponseDTO.Failure(ErrorCodes.Auth.AccountBanned, "Your account has been banned");

            var provider = user.AuthProviders
                .FirstOrDefault(p => p.ProviderType == ProviderTypes.Email
                                  || p.ProviderType == ProviderTypes.Phone);

            if (provider is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            if (!_passwordHasher.VerifyPassword(request.Password, provider.PasswordHash!))
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            if (provider.ProviderType == ProviderTypes.Email && !user.EmailVerified)
                return ResponseDTO.Failure(ErrorCodes.Auth.EmailNotVerified, "Please verify your email before logging in");

            //if (provider.ProviderType == ProviderTypes.Phone && !user.PhoneVerified)
            //    return ResponseDTO.Failure(ErrorCodes.Auth.PhoneNotVerified, "Please verify your phone before logging in");

            var jwt = _jwtService.GenerateToken(user.Id, user.Username.Value, user.Email?.Value, user.Phone?.Value);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var tokenHash = _jwtService.HashToken(jwt);

            var session = LoginSession.Create(
                userId: user.Id,
                loginProvider: provider.ProviderType,
                token: tokenHash,
                refreshToken: refreshToken,
                ipAddress: request.IpAddress,
                userAgent: request.UserAgent,
                deviceType: request.DeviceType,
                expirationHours: _sessionSettings.ExpirationHours);

            await _sessionRepository.AddAsync(session, cancellationToken);

            _logger.LogInformation("Login successful for user {UserId}", user.Id);

            return ResponseDTO.Success(new
            {
                AccessToken = jwt,
                RefreshToken = refreshToken,
                ExpiresAt = session.ExpiresAt,
                User = new
                {
                    user.Id,
                    Username = user.Username.Value,
                    Email = user.Email?.Value,
                    Phone = user.Phone?.Value,
                    Name = user.Name,
                    Avatar = user.Avatar
                }
            }, "Login successful");
        }
    }
}
