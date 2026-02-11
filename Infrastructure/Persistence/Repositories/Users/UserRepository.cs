namespace Daco.Infrastructure.Persistence.Repositories.Users
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

            var userDto = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserDto>(
                UserDbNames.GetUserById,
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

            var userDto = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserDto>(
                UserDbNames.FindUserByEmail,
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

            var userDto = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserDto>(
                UserDbNames.FindUserByPhone,
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
                .Add("p_gender", user.Gender)
                .Add("p_status", user.Status)
                .Add("p_email_verified", user.EmailVerified)
                .Add("p_phone_verified", user.PhoneVerified)
                .AddOutput("o_code", DbType.Int32)
                .AddOutput("o_message", DbType.String)
                .Build();

            await _executor.ExecuteProcedureAsync(
                UserDbNames.CreateUser,
                parameters,
                cancellationToken);

            var code = parameters.Get<int>("o_code");
            var message = parameters.Get<string>("o_message");

            if (code != 0)
            {
                _logger.LogError($"Failed to create user {user.Id}: [{code}] {message}");
                throw new InvalidOperationException($"Failed to create user: {message}");
            }

            _logger.LogInformation($"User {user.Id} created successfully");

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
                .Add("p_gender", user.Gender.ToString())
                .Add("p_status", user.Status.ToString())
                .Add("p_email_verified", user.EmailVerified)
                .Add("p_phone_verified", user.PhoneVerified)
                .AddOutput("o_code", DbType.Int32)
                .AddOutput("o_message", DbType.String)
                .Build();

            await _executor.ExecuteProcedureAsync(
                UserDbNames.UpdateUser,
                parameters,
                cancellationToken);

            var code = parameters.Get<int>("o_code");
            var message = parameters.Get<string>("o_message");

            if (code != 0)
            {
                _logger.LogError($"Failed to update user {user.Id}: [{code}] {message}");
                throw new InvalidOperationException($"Failed to update user: {message}");
            }

            _logger.LogInformation($"User {user.Id} updated successfully");
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
                .AddOutput("o_code", DbType.Int32)
                .AddOutput("o_message", DbType.String)
                .Build();

            await _executor.ExecuteProcedureAsync(
                UserDbNames.CreateAuthProvider,
                parameters,
                cancellationToken);

            var code = parameters.Get<int>("o_code");
            var message = parameters.Get<string>("o_message");

            if (code != 0)
            {
                _logger.LogError($"Failed to create auth provider for user {userId}: [{code}] {message}");
                throw new InvalidOperationException($"Failed to create auth provider: {message}");
            }

            _logger.LogInformation($"Auth provider created successfully for user {userId}");

        }

        private User? MapToDomain(UserDto? dto)
        {
            if (dto == null) return null;

            return User.Reconstitute(
                dto.Id,
                dto.Username,
                dto.Email,
                dto.Phone,
                dto.Name,
                dto.Avatar,
                dto.DateOfBirth,
                (UserGender)dto.Gender,
                (UserStatus)dto.Status,
                dto.EmailVerified,
                dto.PhoneVerified,
                dto.CreatedAt,
                dto.UpdatedAt,
                dto.DeletedAt
            );
        }
    }
}
