namespace Daco.Application.Auth.Queries
{
    public class GetAuthProvidersQueryHandler : IRequestHandler<GetAuthProvidersQuery, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetAuthProvidersQueryHandler> _logger;

        public GetAuthProvidersQueryHandler(
            IUserRepository userRepository,
            ILogger<GetAuthProvidersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(GetAuthProvidersQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting auth providers for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId!.Value, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.User.NotFound, "User not found");

            var providers = user.AuthProviders
                .Where(p => p.DeletedAt == null)
                .Select(p => new
                {
                    p.Id,
                    p.ProviderType,
                    p.IsVerified,
                    p.VerifiedAt,
                    p.CreatedAt,
                    DisplayName = p.ProviderName,
                    DisplayEmail = p.ProviderEmail,
                    Avatar = p.ProviderAvatar,
                    PasswordUpdatedAt = p.PasswordUpdatedAt
                });

            return ResponseDTO.Success(providers);
        }
    }
}
