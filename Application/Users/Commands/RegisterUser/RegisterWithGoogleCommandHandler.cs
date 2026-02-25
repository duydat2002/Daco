namespace Daco.Application.Users.Commands.RegisterUser
{
    public class RegisterWithGoogleCommandHandler : IRequestHandler<RegisterWithGoogleCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterWithGoogleCommandHandler> _logger;

        public RegisterWithGoogleCommandHandler(
            IUserRepository userRepository,
            IGoogleAuthService googleAuthService,
            IUnitOfWork unitOfWork,
            ILogger<RegisterWithGoogleCommandHandler> logger)
        {
            _userRepository = userRepository;
            _googleAuthService = googleAuthService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(
            RegisterWithGoogleCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing RegisterWithGoogle command");

            var googleUser = await _googleAuthService
                .VerifyIdTokenAsync(request.IdToken);

            if (googleUser is null)
            {
                return ResponseDTO.Failure(
                    ErrorCodes.Auth.TokenInvalid,
                    "Invalid or expired Google ID Token");
            }

            if (!googleUser.EmailVerified)
            {
                return ResponseDTO.Failure(
                    ErrorCodes.Auth.EmailNotVerified,
                    "Google email is not verified");
            }

            var providerUserId = googleUser.Subject;
            var email = googleUser.Email;
            var name = googleUser.Name;
            var avatar = googleUser.Picture;

            var user = await _userRepository
                .FindByIdentifierAsync(email, cancellationToken);

            if (user is null)
            {
                var username = UsernameGenerator.GenerateWithSuffix(name);
                while((await _userRepository.FindByIdentifierAsync(username)) != null)
                {
                    username = UsernameGenerator.GenerateWithSuffix(name);
                }

                user = User.CreateWithSocial(
                    username: username,
                    providerType: ProviderTypes.Google,
                    providerUserId: providerUserId,
                    email: email,
                    name: name,
                    avatar: avatar);

                await _userRepository.AddAsync(user, cancellationToken);
            }
            else
            {
                if (!(await _userRepository.CheckUserAuthProvider(user.Id, ProviderTypes.Google, cancellationToken)))
                {
                    user.AddAuthProvider(
                        ProviderTypes.Google,
                        providerUserId,
                        email,
                        googleUser.Name,
                        googleUser.Picture
                    );

                    var newProvider = user.AuthProviders.Last();
                    await _userRepository.AddAuthProviderAsync(user.Id, newProvider, cancellationToken);
                }
            }

            _logger.LogInformation($"Google login successful. UserId: {user.Id}");

            return ResponseDTO.Success(
                new
                {
                    UserId = user.Id,
                    Username = user.Username.Value,
                    Email = user.Email?.Value,
                    Name = user.Name,
                    Avatar = user.Avatar,
                    Provider = ProviderTypes.Google
                },
                "Login successful with Google");
        }
    }
}
