namespace Daco.Application.Users.Commands.AccountStatus
{
    public class SuspendUserCommandHandler : IRequestHandler<SuspendUserCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SuspendUserCommandHandler> _logger;

        public SuspendUserCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<SuspendUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SuspendUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Suspending user {UserId}, reason: {Reason}",
                request.UserId, request.Reason);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            user.Suspend(request.Reason);
            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("User {UserId} suspended", request.UserId);

            return ResponseDTO.Success(null, "User suspended successfully");
        }
    }
}
