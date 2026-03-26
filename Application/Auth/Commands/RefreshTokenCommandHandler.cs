namespace Daco.Application.Auth.Commands
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseDTO>
    {
        private readonly ILoginSessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(
            ILoginSessionRepository sessionRepository,
            IUserRepository userRepository,
            ISellerRepository sellerRepository,
            IJwtService jwtService,
            IUnitOfWork unitOfWork,
            ILogger<RefreshTokenCommandHandler> logger)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
            _sellerRepository = sellerRepository;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing refresh token from IP {IpAddress}", request.IpAddress);

            var session = await _sessionRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (session is null)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.TokenInvalid, "Refresh token không hợp lệ hoặc đã bị thu hồi");

            if (session.IsExpired())
            {
                session.Revoke("expired");
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.TokenExpired, "Refresh token đã hết hạn, vui lòng đăng nhập lại");
            }

            var user = await _userRepository.GetByIdAsync(session.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "Không tìm thấy người dùng");

            session.Revoke("refreshed");

            var roles = new List<string> { "buyer" };
            var isSeller = await _sellerRepository.IsSellerAsync(user.Id, cancellationToken);
            if (isSeller) roles.Add("seller");

            //var isAdmin = await _adminRepository.IsActiveAdminAsync(user.Id, cancellationToken);
            //if (isAdmin) roles.Add("admin");

            var newJwt = _jwtService.GenerateToken(user.Id, user.Username.Value, user.Email?.Value, user.Phone?.Value, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken();
            var newTokenHash = _jwtService.HashToken(newJwt);

            var newSession = LoginSession.Create(
                userId: user.Id,
                loginProvider: session.LoginProvider,
                token: newTokenHash,
                refreshToken: newRefreshToken,
                ipAddress: request.IpAddress,
                userAgent: request.UserAgent ?? session.UserAgent,
                deviceType: request.DeviceType ?? session.DeviceType);

            await _sessionRepository.AddAsync(newSession, cancellationToken);

            _logger.LogInformation("Refresh token thành công cho user {UserId}", user.Id);

            return ResponseDTO.Success(new
            {
                AccessToken = newJwt,
                RefreshToken = newRefreshToken,
                newSession.ExpiresAt,
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
            }, "Token đã được làm mới thành công");
        }
    }
}
