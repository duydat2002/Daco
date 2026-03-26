namespace Daco.Application.Auth.Commands
{
    public class UnlinkProviderCommandHandler : IRequestHandler<UnlinkProviderCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnlinkProviderCommandHandler> _logger;

        public UnlinkProviderCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<UnlinkProviderCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UnlinkProviderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Unlinking {ProviderType} for user {UserId}",
                request.ProviderType, request.UserId);

            var user = await _userRepository.GetByIdWithProvidersAsync(request.UserId!.Value, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            user.UnlinkProvider(request.ProviderType);

            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("{ProviderType} unlinked for user {UserId}",
                request.ProviderType, request.UserId);

            return ResponseDTO.Success(null, $"{request.ProviderType} unlinked successfully");
        }
    }
}
