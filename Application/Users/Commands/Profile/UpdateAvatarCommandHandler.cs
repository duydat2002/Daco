namespace Daco.Application.Users.Commands.Profile
{
    public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAvatarCommandHandler> _logger;

        public UpdateAvatarCommandHandler(
            IUserRepository userRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            ILogger<UpdateAvatarCommandHandler> logger)
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating avatar for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            var oldAvatarUrl = user.Avatar;

            var newAvatarUrl = await _fileStorageService.UploadAvatarAsync(
                request.UserId,
                request.FileStream,
                request.FileName,
                request.ContentType,
                cancellationToken);

            user.UpdateAvatar(newAvatarUrl);
            _unitOfWork.TrackEntity(user);

            if (!string.IsNullOrEmpty(oldAvatarUrl))
            {
                try
                {
                    await _fileStorageService.DeleteAsync(oldAvatarUrl, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete old avatar {OldAvatarUrl}", oldAvatarUrl);
                }
            }

            _logger.LogInformation("Avatar updated for user {UserId}", request.UserId);

            return ResponseDTO.Success(new { AvatarUrl = newAvatarUrl }, "Avatar updated successfully");
        }
    }
}
