namespace Daco.Application.Users.Commands.Profile
{
    public class UpdatePhoneCommandHandler : IRequestHandler<UpdatePhoneCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePhoneCommandHandler> _logger;

        public UpdatePhoneCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdatePhoneCommandHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdatePhoneCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update phone request for user {UserId}", request.UserId);

            var existing = await _userRepository.FindByPhoneAsync(request.Phone, cancellationToken);
            if (existing is not null && existing.Id != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.User.AlreadyExists, "Phone already in use");

            var user = await _userRepository.GetByIdWithProvidersAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            user.UpdatePhone(request.Phone); 
            _unitOfWork.TrackEntity(user);

            return ResponseDTO.Success(new { Phone = request.Phone },
                "Phone updated. Please verify your new phone with the OTP sent.");

        }
    }
}
