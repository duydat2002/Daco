namespace Daco.Application.Administration.Auth
{
    public class AdminVerifyOtpCommandHandler : IRequestHandler<AdminVerifyOtpCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAdminUserRepository _adminRepository;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        private readonly SessionSettings _sessionSettings;
        private readonly ILogger<AdminVerifyOtpCommandHandler> _logger;

        public AdminVerifyOtpCommandHandler(
            IUserRepository userRepository,
            IAdminUserRepository adminRepository,
            IVerificationTokenRepository tokenRepository,
            ILoginSessionRepository sessionRepository,
            IJwtService jwtService,
            IOptions<SessionSettings> sessionSettings,
            ILogger<AdminVerifyOtpCommandHandler> logger)
        {
            _userRepository = userRepository;
            _adminRepository = adminRepository;
            _tokenRepository = tokenRepository;
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
            _sessionSettings = sessionSettings.Value;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AdminVerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var userId = _jwtService.ValidateTempToken(request.TempToken);
            if (userId is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.TokenInvalid, "Invalid or expired token");

            var user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            var adminUser = await _adminRepository.GetByUserIdAsync(userId.Value, cancellationToken);
            if (adminUser is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

            var isValid = await _tokenRepository.ValidateTokenAsync(
                userId.Value,
                request.Otp,
                cancellationToken);

            if (!isValid)
                return ResponseDTO.Failure(ErrorCodes.Auth.TokenInvalid, "Invalid or expired OTP");

            var adminRoles = await _adminRepository.GetRolesAsync(adminUser.Id, cancellationToken);

            var roles = new List<string> { UserRoles.Admin };
            roles.AddRange(adminRoles);

            var jwt = _jwtService.GenerateToken(user.Id, user.Username.Value, user.Email?.Value, user.Phone?.Value, roles);
            var refreshToken = _jwtService.GenerateRefreshToken();
            var tokenHash = _jwtService.HashToken(jwt);

            var session = LoginSession.Create(
                userId: user.Id,
                loginProvider: ProviderTypes.Email,
                token: tokenHash,
                refreshToken: refreshToken,
                ipAddress: request.IpAddress,
                userAgent: request.UserAgent,
                deviceType: request.DeviceType,
                expirationHours: _sessionSettings.ExpirationHours);

            await _sessionRepository.AddAsync(session, cancellationToken);

            _logger.LogInformation("Admin login successful: {AdminId}", adminUser.Id);

            return ResponseDTO.Success(new
            {
                AccessToken = jwt,
                RefreshToken = refreshToken,
                ExpiresAt = session.ExpiresAt,
                Admin = new
                {
                    adminUser.Id,
                    UserId = user.Id,
                    Username = user.Username.Value,
                    Email = user.Email?.Value,
                    EmployeeCode = adminUser.EmployeeCode,
                    Department = adminUser.Department,
                    Position = adminUser.Position,
                    Roles = adminRoles
                }
            }, "Login successful");
        }
    }
}
