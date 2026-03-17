namespace Daco.Application.Users.Commands.Profile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        public UpdateUserProfileCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateUserProfileCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating profile for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            var dateOfBirth = DateUtils.ParseToUtc(request.DateOfBirth);

            user.UpdateProfile(request.Name, dateOfBirth, request.Gender);

            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("Profile updated for user {UserId}", request.UserId);

            return ResponseDTO.Success(new
            {
                user.Id,
                Username = user.Username.Value,
                user.Name,
                user.DateOfBirth,
                user.Gender,
                user.Avatar,
                user.UpdatedAt
            }, "Profile updated successfully");
        }
    }
}
