namespace Daco.Application.Administration.UserManagement.Commands
{
    public class SuspendUserCommandHandler : IRequestHandler<SuspendUserCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SuspendUserCommand> _logger;

        public SuspendUserCommandHandler(
            IAdminUserRepository adminRepository, 
            IUserRepository userRepository, 
            IUnitOfWork unitOfWork,
            ILogger<SuspendUserCommand> logger)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SuspendUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin suspending user {UserId}, reason: {Reason}",
                request.UserId, request.Reason);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            if (user.Status == UserStatus.Suspended)
                return ResponseDTO.Failure(ErrorCodes.Auth.AccountSuspended, "User is already suspended");

            if (user.Status == UserStatus.Banned)
                return ResponseDTO.Failure(ErrorCodes.Auth.AccountBanned, "Cannot suspend a banned user");

            user.Suspend(request.Reason);
            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("User {UserId} suspended by admin", request.UserId);

            return ResponseDTO.Success(new
            {
                request.UserId,
                request.Reason,
                Status = UserStatus.Suspended.ToString().ToLower()
            }, "User suspended successfully");
        }
    }
}
