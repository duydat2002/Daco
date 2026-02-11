namespace Daco.Infrastructure.Services.Authentication
{
    public class VerificationTokenService : IVerificationTokenService
    {
        private readonly DapperExecutor _executor;
        private readonly ILogger<VerificationTokenService> _logger;

        public VerificationTokenService(
            DapperExecutor executor,
            ILogger<VerificationTokenService> logger)
        {
            _executor = executor;
            _logger = logger;
        }

        public async Task<string> GenerateTokenAsync(
            Guid userId,
            string tokenType,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Generating verification token for user {userId}, type {tokenType}");

            var token = Random.Shared.Next(100000, 999999).ToString();

            var parameters = new DapperParameterBuilder()
                .Add("p_user_id", userId)
                .Add("p_token", token)
                .Add("p_token_type", tokenType)
                .Build();

            await _executor.ExecuteProcedureAsync(
                "sp_create_verification_token",
                parameters,
                cancellationToken);

            return token;
        }

        public async Task<bool> ValidateTokenAsync(
            Guid userId,
            string token,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Validating token for user {userId}");

            var parameters = new DapperParameterBuilder()
                .Add("p_user_id", userId)
                .Add("p_token", token)
                .Build();

            var result = await _executor.ExecuteFunctionScalarAsync<bool>(
                "sp_validate_verification_token",
                parameters,
                cancellationToken);

            return result;
        }
    }
}
