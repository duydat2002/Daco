namespace Daco.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Registering user with username: {request.Username}");

            if (!string.IsNullOrEmpty(request.Email))
            {
                var existingUser = await _userRepository.FindByEmailAsync(request.Email, cancellationToken);
                if (existingUser != null)
                    return ResponseDTO.Failure(ErrorCodes.Auth.UserAlreadyExists, "The email address has already been used!");
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                var existingUser = await _userRepository.FindByPhoneAsync(request.Phone, cancellationToken);
                if (existingUser != null)
                return ResponseDTO.Failure(ErrorCodes.Auth.UserAlreadyExists, "The phone number has already been used!");
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);

            User user;
            if (!string.IsNullOrEmpty(request.Email))
            {
                user = User.CreateWithEmail(
                    request.Username,
                    request.Email,
                    passwordHash);
            }
            else
            {
                user = User.CreateWithPhone(
                    request.Username,
                    request.Phone!,
                    passwordHash);
            }

            _logger.LogDebug("User domain model created with {EventCount} events",
                user.DomainEvents.Count);

            await _userRepository.AddAsync(user, cancellationToken);

            _unitOfWork.TrackEntity(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("User registered successfully: {UserId}", user.Id);

            return ResponseDTO.Success(
                new {
                    UserId = user.Id,
                    Username = user.Username.Value,
                    Email = user.Email?.Value,
                    Phone = user.Phone?.Value
                }, "User registered successfully. Please verify your email/phone.");
        }
    }
}
