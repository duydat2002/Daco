namespace Daco.Application.Auth.Commands
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IJwtService _jwtService;
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VerifyEmailCommandHandler> _logger;

        public VerifyEmailCommandHandler(
             IUserRepository userRepository,
             IVerificationTokenRepository tokenRepository,
             ISellerRepository sellerRepository,
             IJwtService jwtService,
             ILoginSessionRepository sessionRepository,
             IUnitOfWork unitOfWork,
             ILogger<VerifyEmailCommandHandler> logger)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _sellerRepository = sellerRepository;
            _jwtService = jwtService;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Verifying email OTP for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdWithProvidersAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            if (user.EmailVerified)
                return ResponseDTO.Failure(ErrorCodes.Auth.EmailNotVerified, "Email is already verified");

            var isValid = await _tokenRepository.ValidateTokenAsync(
                request.UserId,
                request.Otp,
                cancellationToken);

            if (!isValid)
                return ResponseDTO.Failure(ErrorCodes.Auth.TokenInvalid, "OTP is invalid or expired");

            user.VerifyEmail();
            await _userRepository.UpdateAsync(user, cancellationToken);

            var roles = new List<string> { "buyer" };
            var userRoles = await _userRepository.GetUserRolesAsync(user.Id);
            if (userRoles.IsSeller) roles.Add("seller");
            if (userRoles.IsAdmin)
                return ResponseDTO.Failure(ErrorCodes.Auth.InvalidCredentials, "Invalid credentials");

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
                deviceType: request.DeviceType);

            await _sessionRepository.AddAsync(session, cancellationToken);

            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("Email verified successfully for user {UserId}", request.UserId);

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
            }, "Email verified successfully");
        }
    }
}
