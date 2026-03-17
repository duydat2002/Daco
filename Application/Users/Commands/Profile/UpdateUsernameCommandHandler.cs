namespace Daco.Application.Users.Commands.Profile
{
    public class UpdateUsernameCommandHandler : IRequestHandler<UpdateUsernameCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUsernameCommandHandler> _logger;

        public UpdateUsernameCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateUsernameCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateUsernameCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating username for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");
            
            var existing = await _userRepository.FindByIdentifierAsync(request.Username, cancellationToken);
            if (existing is not null && existing.Id != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.User.AlreadyExists, "Username already taken");

            user.UpdateUsername(request.Username);
            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("Username updated for user {UserId}", request.UserId);

            return ResponseDTO.Success(new { Username = user.Username.Value }, "Username updated successfully");
        }
    }
}
