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

            var email = googleUser.Email;
            var providerUserId = googleUser.Subject;

            var user = await _userRepository
                .FindByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                var username = await GenerateUniqueUsernameAsync(
                    googleUser,
                    cancellationToken);

                user = User.CreateWithSocial(
                    username: username,
                    providerType: ProviderTypes.Google,
                    providerUserId: providerUserId,
                    email: email,
                    name: googleUser.Name,
                    avatar: googleUser.Picture);

                await _userRepository.AddAsync(user, cancellationToken);
            }
            //else
            //{
            //    if (!user.HasProvider(ProviderTypes.Google))
            //    {
            //        user.AddAuthProvider(
            //            ProviderTypes.Google,
            //            providerUserId);
            //    }
            //}

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Google login successful. UserId: {UserId}",
                user.Id);

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

        private async Task<string> GenerateUniqueUsernameAsync(
            GoogleUserInfo googleUser,
            CancellationToken cancellationToken)
        {
            var source = !string.IsNullOrWhiteSpace(googleUser.Name)
                ? googleUser.Name
                : googleUser.Email.Split('@')[0];

            var baseUsername = new string(
                source.ToLowerInvariant()
                      .Select(c => char.IsLetterOrDigit(c) ? c : '_')
                      .ToArray())
                .Trim('_');

            if (baseUsername.Length < 3)
                baseUsername = "user";

            if (baseUsername.Length > 40)
                baseUsername = baseUsername[..40];

            var username = baseUsername;
            var suffix = 1;

            //while (await _userRepository
            //    .UsernameExistsAsync(username, cancellationToken))
            //{
            //    username = $"{baseUsername}_{suffix++}";
            //}

            return username;
        }
    }
}
