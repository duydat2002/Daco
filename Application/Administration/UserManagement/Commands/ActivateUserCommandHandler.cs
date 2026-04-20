namespace Daco.Application.Administration.UserManagement.Commands
{
    public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ActivateUserCommandHandler> _logger;

        public ActivateUserCommandHandler(
            IAdminUserRepository adminRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<ActivateUserCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin activating user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            if (user.Status == UserStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.AlreadyActivated, "User is already active");

            if (user.Status == UserStatus.Deleted)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            user.Activate();
            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("User {UserId} activated by admin", request.UserId);

            return ResponseDTO.Success(new
            {
                request.UserId,
                Status = UserStatus.Active.ToString().ToLower()
            }, "User activated successfully");
        }
    }
}
