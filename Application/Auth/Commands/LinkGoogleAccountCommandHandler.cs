namespace Daco.Application.Auth.Commands
{
    public class LinkGoogleAccountCommandHandler : IRequestHandler<LinkGoogleAccountCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LinkGoogleAccountCommandHandler> _logger;

        public LinkGoogleAccountCommandHandler(
            IUserRepository userRepository,
            IGoogleAuthService googleAuthService,
            IUnitOfWork unitOfWork,
            ILogger<LinkGoogleAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _googleAuthService = googleAuthService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(LinkGoogleAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Linking Google account for user {UserId}", request.UserId);

            var googleUser = await _googleAuthService.VerifyIdTokenAsync(request.IdToken);
            if (googleUser is null)
                return ResponseDTO.Failure(ErrorCodes.Auth.TokenInvalid, "Invalid or expired Google ID token");

            if (!googleUser.EmailVerified)
                return ResponseDTO.Failure(ErrorCodes.Auth.EmailNotVerified, "Google email is not verified");

            var user = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            var existingUser = await _userRepository.FindByIdentifierAsync(googleUser.Email, cancellationToken);
            if (existingUser is not null && existingUser.Id != user.Id)
                return ResponseDTO.Failure(ErrorCodes.Auth.UserAlreadyExists, "This Google account is already linked to another user");

            if (await _userRepository.CheckUserAuthProvider(user.Id, ProviderTypes.Google, cancellationToken))
                return ResponseDTO.Failure(ErrorCodes.Auth.UserAlreadyExists, "Google account is already linked");

            var countBefore = user.AuthProviders.Count;

            user.AddAuthProvider(
                ProviderTypes.Google,
                googleUser.Subject,
                googleUser.Email,
                googleUser.Name,
                googleUser.Picture);

            var isNewProvider = user.AuthProviders.Count > countBefore;
            if (isNewProvider)
            {
                var newProvider = user.AuthProviders.Last();
                await _userRepository.AddAuthProviderAsync(user.Id, newProvider, cancellationToken);
            }
            _unitOfWork.TrackEntity(user);

            _logger.LogInformation("Google account linked successfully for user {UserId}", user.Id);

            return ResponseDTO.Success(new
            {
                Provider = ProviderTypes.Google,
                googleUser.Email,
                googleUser.Name,
                Avatar = googleUser.Picture
            }, "Google account linked successfully");
        }
    }
}
