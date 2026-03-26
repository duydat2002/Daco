namespace Daco.Application.Users.Commands.Profile
{
    public class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationTokenRepository _tokenRepository;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateEmailCommandHandler> _logger;

        public UpdateEmailCommandHandler(
            IUserRepository userRepository,
            IVerificationTokenRepository tokenRepository,
            IEmailService emailService,
            IUnitOfWork unitOfWork,
            ILogger<UpdateEmailCommandHandler> logger)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
        {
            var existing = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);
            if (existing is not null && existing.Id != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.AlreadyExists, "Email already in use");

            var user = await _userRepository.GetByIdWithProvidersAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            user.UpdateEmail(request.Email);
            _unitOfWork.TrackEntity(user);

            return ResponseDTO.Success(new { Email = request.Email },
                "Email updated. Please verify your new email with the OTP sent.");
        }
    }
}
