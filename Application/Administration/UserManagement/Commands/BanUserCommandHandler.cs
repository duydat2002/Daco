namespace Daco.Application.Administration.UserManagement.Commands
{
    public class BanUserCommandHandler : IRequestHandler<BanUserCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BanUserCommandHandler> _logger;

        public BanUserCommandHandler(
            IAdminUserRepository adminRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<BanUserCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin banning user {UserId}, reason: {Reason}",
                request.UserId, request.Reason);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            if (user.Status == UserStatus.Banned)
                return ResponseDTO.Failure(ErrorCodes.Auth.AccountSuspended, "User is already banned");

            user.Ban(request.Reason);
            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("User {UserId} banned by admin", request.UserId);

            return ResponseDTO.Success(new
            {
                request.UserId,
                request.Reason,
                Status = UserStatus.Banned.ToString().ToLower()
            }, "User banned successfully");
        }
    }
}
