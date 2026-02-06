namespace Daco.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperExecutor _executor;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            DapperExecutor executor,
            ILogger<UserRepository> logger)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Getting user by id: {id}");

            var parameters = new DapperParameterBuilder()
                .Add("p_user_id", id)
                .Build();

            var userDto = await _executor.QuerySingleOrDefaultAsync<UserDto>(
                "sp_get_user_by_id",
                parameters,
                cancellationToken);

            if (userDto == null)
                return null;

            return MapToDomain(userDto);
        }

        public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Finding user by email: {email}");

            var parameters = new DapperParameterBuilder()
                .Add("p_email", email)
                .Build();

            var userDto = await _executor.QuerySingleOrDefaultAsync<UserDto>(
                "sp_find_user_by_email",
                parameters,
                cancellationToken);

            if (userDto == null)
                return null;

            return MapToDomain(userDto);
        }

        public async Task<User?> FindByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Finding user by phone: {phone}");

            var parameters = new DapperParameterBuilder()
                .Add("p_phone", phone)
                .Build();

            var userDto = await _executor.QuerySingleOrDefaultAsync<UserDto>(
                "sp_find_user_by_phone",
                parameters,
                cancellationToken);

            if (userDto == null)
                return null;

            return MapToDomain(userDto);
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Adding user: {user.Id}");

            var parameters = new DapperParameterBuilder()
                .Add("p_user_id", user.Id)
                .Add("p_username", user.Username.Value)
                .Add("p_email", user.Email?.Value)
                .Add("p_phone", user.Phone?.Value)
                .Add("p_name", user.Name)
                .Add("p_avatar", user.Avatar)
                .Add("p_date_of_birth", user.DateOfBirth)
                .Add("p_gender", user.Gender?.ToString())
                .Add("p_status", user.Status.ToString())
                .Add("p_email_verified", user.EmailVerified)
                .Add("p_phone_verified", user.PhoneVerified)
                .Build();

            await _executor.ExecuteAsync(
                "sp_create_user",
                parameters,
                cancellationToken);

            foreach (var provider in user.AuthProviders)
            {
                await AddAuthProviderAsync(user.Id, provider, cancellationToken);
            }
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Updating user: {user.Id}");

            var parameters = new DapperParameterBuilder()
                .Add("p_user_id", user.Id)
                .Add("p_username", user.Username.Value)
                .Add("p_email", user.Email?.Value)
                .Add("p_phone", user.Phone?.Value)
                .Add("p_name", user.Name)
                .Add("p_avatar", user.Avatar)
                .Add("p_date_of_birth", user.DateOfBirth)
                .Add("p_gender", user.Gender?.ToString())
                .Add("p_status", user.Status.ToString())
                .Add("p_email_verified", user.EmailVerified)
                .Add("p_phone_verified", user.PhoneVerified)
                .Build();

            await _executor.ExecuteAsync(
                "sp_update_user",
                parameters,
                cancellationToken);
        }

        private async Task AddAuthProviderAsync(
            Guid userId,
            AuthProvider provider,
            CancellationToken cancellationToken)
        {
            var parameters = new DapperParameterBuilder()
                .Add("p_auth_provider_id", provider.Id)
                .Add("p_user_id", userId)
                .Add("p_provider_type", provider.ProviderType.ToString())
                .Add("p_provider_key", provider.ProviderKey)
                .Add("p_password_hash", provider.PasswordHash)
                .Add("p_provider_user_id", provider.ProviderUserId)
                .Add("p_provider_email", provider.ProviderEmail)
                .Add("p_provider_name", provider.ProviderName)
                .Add("p_provider_avatar", provider.ProviderAvatar)
                .Add("p_is_verified", provider.IsVerified)
                .Build();

            await _executor.ExecuteAsync(
                "sp_create_auth_provider",
                parameters,
                cancellationToken);
        }

        private User MapToDomain(UserDto dto)
        {
            throw new NotImplementedException("Mapping from DTO to Domain not implemented yet");
        }

        private class UserDto
        {
            public Guid Id { get; set; }
            public string Username { get; set; } = null!;
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Name { get; set; }
            public string? Avatar { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string? Gender { get; set; }
            public string Status { get; set; } = null!;
            public bool EmailVerified { get; set; }
            public bool PhoneVerified { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public DateTime? DeletedAt { get; set; }
        }
    }
}
