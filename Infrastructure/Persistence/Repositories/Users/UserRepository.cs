using Daco.Application.Common.DTOs;

namespace Daco.Infrastructure.Persistence.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly DapperExecutor _executor;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(
            AppDbContext context,
            DapperExecutor executor,
            ILogger<UserRepository> logger)
        {
            _context = context;
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region EF
        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, user,
                async () => await _context.Users.AddAsync(user, cancellationToken));
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            await RepositoryLogger.ExecuteAsync(_logger, user,
                () => 
                {
                    _context.Users.Update(user);
                    return Task.CompletedTask;
                });
        }

        public async Task AddAuthProviderAsync(
            Guid userId,
            AuthProvider provider,
            CancellationToken cancellationToken)
        {
            await RepositoryLogger.ExecuteAsync(_logger, provider,
                async () => await _context.Set<AuthProvider>().AddAsync(provider, cancellationToken));
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id, cancellationToken));
        }

        public async Task<User?> GetByIdWithProvidersAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { id },
                () => _context.Users
                .Include(u => u.AuthProviders)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken));
        }

        public async Task<User?> FindByIdentifierAsync(string identifier, CancellationToken cancellationToken = default)
        {
            var normalized = identifier.Trim().ToLowerInvariant();
            return await RepositoryLogger.ExecuteAsync(_logger, new { identifier },
                () => _context.Users
                    .Include(u => u.AuthProviders)
                    .Where(u => u.DeletedAt == null &&
                        (u.Username.Value == normalized ||
                        u.Email.Value == normalized ||
                        u.Phone.Value == normalized))
                    .FirstOrDefaultAsync(cancellationToken));
        }

        public async Task<User?> FindByEmailAndPhoneAsync(string email, string phone, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { email },
                () => _context.Users
                    .Include(u => u.AuthProviders)
                    .Where(u => u.DeletedAt == null && u.Email.Value == email && u.Phone.Value == phone)
                    .FirstOrDefaultAsync(cancellationToken));
        }

        public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { email },
                () => _context.Users
                    .Include(u => u.AuthProviders)
                    .Where(u => u.DeletedAt == null && u.Email.Value == email)
                    .FirstOrDefaultAsync(cancellationToken));
        }

        public async Task<User?> FindByPhoneAsync(string phone, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { phone },
                () => _context.Users
                    .Include(u => u.AuthProviders)
                    .Where(u => u.DeletedAt == null && u.Phone.Value == phone)
                    .FirstOrDefaultAsync(cancellationToken));
        }

        public async Task<bool> CheckUserAuthProvider(
            Guid userId,
            string providerType,
            CancellationToken cancellationToken)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { userId, providerType },
                () => _context.Set<AuthProvider>()
                    .AnyAsync(a => a.UserId == userId &&
                                   a.ProviderType == providerType &&
                                   a.DeletedAt == null, cancellationToken));
        }
        #endregion

        #region Procedure
        public async Task<UserRolesDTO> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await RepositoryLogger.ExecuteAsync(_logger, new { userId },
                async () =>
                {
                    var parameters = new DapperParameterBuilder()
                        .Add("p_user_id", userId)
                        .Build();

                    var result = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserRolesDTO>(
                        UserDbNames.GetUserRoles,
                        parameters,
                        cancellationToken);

                    return result ?? new UserRolesDTO();
                });
        }
        #endregion

        //#region Procedure
        //public async Task<User?> GetByIdAsync1(Guid id, CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Getting user by id: {id}");

        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_user_id", id)
        //        .Build();

        //    var userDto = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserDto>(
        //        UserDbNames.GetUserById,
        //        parameters,
        //        cancellationToken);

        //    if (userDto == null)
        //        return null;

        //    return MapToDomain(userDto);
        //}

        //public async Task<User?> FindByIdentifierAsync1(string identifier, CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Finding user by email: {identifier}");

        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_identifier", identifier)
        //        .Build();

        //    var userDto = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserDto>(
        //        UserDbNames.FindUserByIdentifier,
        //        parameters,
        //        cancellationToken);

        //    if (userDto == null)
        //        return null;

        //    return MapToDomain(userDto);
        //}

        //public async Task<User?> FindByEmailAsync1(string email, CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Finding user by email: {email}");

        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_email", email)
        //        .Build();

        //    var userDto = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserDto>(
        //        UserDbNames.FindUserByEmail,
        //        parameters,
        //        cancellationToken);

        //    if (userDto == null)
        //        return null;

        //    return MapToDomain(userDto);
        //}

        //public async Task<User?> FindByPhoneAsync1(string phone, CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Finding user by phone: {phone}");

        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_phone", phone)
        //        .Build();

        //    var userDto = await _executor.ExecuteFunctionSingleOrDefaultAsync<UserDto>(
        //        UserDbNames.FindUserByPhone,
        //        parameters,
        //        cancellationToken);

        //    if (userDto == null)
        //        return null;

        //    return MapToDomain(userDto);
        //}

        //public async Task AddAsync1(User user, CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Adding user: {user.Id}");

        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_user_id", user.Id)
        //        .Add("p_username", user.Username.Value)
        //        .Add("p_email", user.Email?.Value)
        //        .Add("p_phone", user.Phone?.Value)
        //        .Add("p_name", user.Name)
        //        .Add("p_avatar", user.Avatar)
        //        .Add("p_date_of_birth", user.DateOfBirth)
        //        .Add("p_gender", user.Gender)
        //        .Add("p_status", user.Status)
        //        .Add("p_email_verified", user.EmailVerified)
        //        .Add("p_phone_verified", user.PhoneVerified)
        //        .AddOutput("o_code", DbType.Int32)
        //        .AddOutput("o_message", DbType.String)
        //        .Build();

        //    await _executor.ExecuteProcedureAsync(
        //        UserDbNames.CreateUser,
        //        parameters,
        //        cancellationToken);

        //    var code = parameters.Get<int>("o_code");
        //    var message = parameters.Get<string>("o_message");

        //    if (code != 0)
        //    {
        //        _logger.LogError($"Failed to create user {user.Id}: [{code}] {message}");
        //        throw new InvalidOperationException($"Failed to create user: {message}");
        //    }

        //    _logger.LogInformation($"User {user.Id} created successfully");

        //    foreach (var provider in user.AuthProviders)
        //    {
        //        await AddAuthProviderAsync(user.Id, provider, cancellationToken);
        //    }
        //}

        //public async Task UpdateAsync1(User user, CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Updating user: {user.Id}");

        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_user_id", user.Id)
        //        .Add("p_username", user.Username.Value)
        //        .Add("p_email", user.Email?.Value)
        //        .Add("p_phone", user.Phone?.Value)
        //        .Add("p_name", user.Name)
        //        .Add("p_avatar", user.Avatar)
        //        .Add("p_date_of_birth", user.DateOfBirth)
        //        .Add("p_gender", user.Gender.ToString())
        //        .Add("p_status", user.Status.ToString())
        //        .Add("p_email_verified", user.EmailVerified)
        //        .Add("p_phone_verified", user.PhoneVerified)
        //        .AddOutput("o_code", DbType.Int32)
        //        .AddOutput("o_message", DbType.String)
        //        .Build();

        //    await _executor.ExecuteProcedureAsync(
        //        UserDbNames.UpdateUser,
        //        parameters,
        //        cancellationToken);

        //    var code = parameters.Get<int>("o_code");
        //    var message = parameters.Get<string>("o_message");

        //    if (code != 0)
        //    {
        //        _logger.LogError($"Failed to update user {user.Id}: [{code}] {message}");
        //        throw new InvalidOperationException($"Failed to update user: {message}");
        //    }

        //    _logger.LogInformation($"User {user.Id} updated successfully");
        //}

        //public async Task AddAuthProviderAsync1(
        //    Guid userId,
        //    AuthProvider provider,
        //    CancellationToken cancellationToken)
        //{
        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_auth_provider_id", provider.Id)
        //        .Add("p_user_id", userId)
        //        .Add("p_provider_type", provider.ProviderType.ToString())
        //        .Add("p_provider_key", provider.ProviderKey)
        //        .Add("p_password_hash", provider.PasswordHash)
        //        .Add("p_provider_user_id", provider.ProviderUserId)
        //        .Add("p_provider_email", provider.ProviderEmail)
        //        .Add("p_provider_name", provider.ProviderName)
        //        .Add("p_provider_avatar", provider.ProviderAvatar)
        //        .Add("p_is_verified", provider.IsVerified)
        //        .AddOutput("o_code", DbType.Int32)
        //        .AddOutput("o_message", DbType.String)
        //        .Build();

        //    await _executor.ExecuteProcedureAsync(
        //        UserDbNames.CreateAuthProvider,
        //        parameters,
        //        cancellationToken);

        //    var code = parameters.Get<int>("o_code");
        //    var message = parameters.Get<string>("o_message");

        //    if (code != 0)
        //    {
        //        _logger.LogError($"Failed to create auth provider for user {userId}: [{code}] {message}");
        //        throw new InvalidOperationException($"Failed to create auth provider: {message}");
        //    }

        //    _logger.LogInformation($"Auth provider created successfully for user {userId}");

        //}

        //public async Task<bool> CheckUserAuthProvider1(
        //    Guid userId,
        //    string providerType,
        //    CancellationToken cancellationToken)
        //{
        //    var parameters = new DapperParameterBuilder()
        //        .Add("p_user_id", userId)
        //        .Add("p_provider_type", providerType)
        //        .Build();

        //    var result = await _executor.ExecuteFunctionScalarAsync<bool>(
        //        UserDbNames.CheckUserAuthProvider,
        //        parameters,
        //        cancellationToken);

        //    _logger.LogInformation($"CheckUserAuthProvider: \nOutput: {result}");

        //    return result;
        //}

        //private User? MapToDomain(UserDto? dto)
        //{
        //    if (dto == null) return null;

        //    return User.Reconstitute(
        //        dto.Id,
        //        dto.Username,
        //        dto.Email,
        //        dto.Phone,
        //        dto.Name,
        //        dto.Avatar,
        //        dto.DateOfBirth,
        //        (UserGender)dto.Gender,
        //        (UserStatus)dto.Status,
        //        dto.EmailVerified,
        //        dto.PhoneVerified,
        //        dto.CreatedAt,
        //        dto.UpdatedAt,
        //        dto.DeletedAt
        //    );
        //}
        //#endregion
    }
}
