namespace Daco.Application.Auth.Commands
{
    public class LoginWithGoogleCommandHandler : IRequestHandler<LoginWithGoogleCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly SessionSettings _sessionSettings;
        private readonly ILogger<LoginWithGoogleCommandHandler> _logger;

        public LoginWithGoogleCommandHandler(
            IUserRepository userRepository,
            ILoginSessionRepository sessionRepository,
            ISellerRepository sellerRepository,
            IGoogleAuthService googleAuthService,
            IJwtService jwtService,
            IUnitOfWork unitOfWork,
            IOptions<SessionSettings> sessionSettings,
            ILogger<LoginWithGoogleCommandHandler> logger)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _sellerRepository = sellerRepository;
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
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.TokenInvalid, "Invalid or expired Google ID token");

            if (!googleUser.EmailVerified)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.EmailNotVerified, "Google email is not verified");

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
                    return ResponseDTO.Failure(ErrorCodes.AuthErrors.AccountSuspended, "Your account has been suspended");

                if (user.Status == UserStatus.Banned)
                    return ResponseDTO.Failure(ErrorCodes.AuthErrors.AccountBanned, "Your account has been banned");

                if (!await _userRepository.CheckUserAuthProvider(user.Id, ProviderTypes.Google, cancellationToken))
                {
                    user.AddAuthProvider(ProviderTypes.Google, providerUserId, email, googleUser.Name, googleUser.Picture);
                    var newProvider = user.AuthProviders.Last();
                    await _userRepository.AddAuthProviderAsync(user.Id, newProvider, cancellationToken);
                    _unitOfWork.TrackEntity(user);
                }

                _logger.LogInformation("Existing user logged in via Google: {UserId}", user.Id);
            }

            var roles = new List<string> { "buyer" };
            var userRoles = await _userRepository.GetUserRolesAsync(user.Id);
            if (userRoles.IsSeller) roles.Add("seller");
            if (userRoles.IsAdmin)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.InvalidCredentials, "Invalid credentials");

            var jwt = _jwtService.GenerateToken(user.Id, user.Username.Value, user.Email?.Value, user.Phone?.Value, roles);
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
                    user.Avatar,
                    Roles = roles
                }
            }, "Login successful");
        }
    }
}
