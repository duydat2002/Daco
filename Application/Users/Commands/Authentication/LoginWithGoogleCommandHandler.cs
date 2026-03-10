namespace Daco.Application.Users.Commands.Authentication
{
    public class LoginWithGoogleCommandHandler : IRequestHandler<LoginWithGoogleCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SessionSettings _sessionSettings;
        private readonly ILogger<LoginWithGoogleCommandHandler> _logger;

        public LoginWithGoogleCommandHandler(
            IUserRepository userRepository,
            ILoginSessionRepository sessionRepository,
            IGoogleAuthService googleAuthService,
            IJwtService jwtService,
            IUnitOfWork unitOfWork,
            IOptions<SessionSettings> sessionSettings,
            ILogger<LoginWithGoogleCommandHandler> logger)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _googleAuthService = googleAuthService;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _sessionSettings = sessionSettings.Value;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(
            LoginWithGoogleCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing LoginWithGoogle command");

            var googleUser = await _googleAuthService.VerifyIdTokenAsync(request.IdToken);
            if (googleUser is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.TokenInvalid, "Invalid or expired Google ID token");

            if (!googleUser.EmailVerified)
                return ResponseDTO.Failure(ErrorCodes.Auth.EmailNotVerified, "Google email is not verified");

            var providerUserId = googleUser.Subject;
            var email = googleUser.Email;

            var user = await _userRepository.FindByIdentifierAsync(email, cancellationToken);

            if (user is null)
            {
                var username = UsernameGenerator.GenerateWithSuffix(googleUser.Name);
                while (await _userRepository.FindByIdentifierAsync(username) != null)
                    username = UsernameGenerator.GenerateWithSuffix(googleUser.Name);

                user = User.CreateWithSocial(
                    username: username,
                    providerType: ProviderTypes.Google,
                    providerUserId: providerUserId,
                    email: email,
                    name: googleUser.Name,
                    avatar: googleUser.Picture);

                await _userRepository.AddAsync(user, cancellationToken);
                _unitOfWork.TrackEntity(user);

                _logger.LogInformation("New user created via Google: {UserId}", user.Id);
            }
            else
            {
                if (user.Status == UserStatus.Suspended)
                    return ResponseDTO.Failure(ErrorCodes.Auth.AccountSuspended, "Your account has been suspended");

                if (user.Status == UserStatus.Banned)
                    return ResponseDTO.Failure(ErrorCodes.Auth.AccountBanned, "Your account has been banned");

                if (!await _userRepository.CheckUserAuthProvider(user.Id, ProviderTypes.Google, cancellationToken))
                {
                    user.AddAuthProvider(ProviderTypes.Google, providerUserId, email, googleUser.Name, googleUser.Picture);
                    var newProvider = user.AuthProviders.Last();
                    await _userRepository.AddAuthProviderAsync(user.Id, newProvider, cancellationToken);
                    _unitOfWork.TrackEntity(user);
                }

                _logger.LogInformation("Existing user logged in via Google: {UserId}", user.Id);
            }

            var jwt = _jwtService.GenerateToken(user.Id, user.Username.Value, user.Email?.Value, user.Phone?.Value);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var tokenHash = _jwtService.HashToken(jwt);

            var session = LoginSession.Create(
                userId: user.Id,
                loginProvider: ProviderTypes.Google,
                token: tokenHash,
                refreshToken: refreshToken,
                ipAddress: request.IpAddress,
                userAgent: request.UserAgent,
                deviceType: request.DeviceType,
                expirationHours: _sessionSettings.ExpirationHours);

            await _sessionRepository.AddAsync(session, cancellationToken);

            return ResponseDTO.Success(new
            {
                AccessToken = jwt,
                RefreshToken = refreshToken,
                session.ExpiresAt,
                User = new
                {
                    user.Id,
                    Username = user.Username.Value,
                    Email = user.Email?.Value,
                    Phone = user.Phone?.Value,
                    user.Name,
                    user.Avatar
                }
            }, "Login successful");
        }
    }
}
