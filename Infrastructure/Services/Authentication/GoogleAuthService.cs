namespace Daco.Infrastructure.Services.Authentication
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly ILogger<GoogleAuthService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _clientId;

        public GoogleAuthService(
            ILogger<GoogleAuthService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _clientId = _configuration["Google:ClientId"]
                ?? throw new InvalidOperationException("Google ClientId not configured");
        }

        public async Task<GoogleUserInfo?> VerifyIdTokenAsync(string idToken)
        {
            _logger.LogInformation("Verifying Google ID token");

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                _logger.LogInformation(
                    "Google ID token verified. Subject: {Subject}, Email: {Email}, EmailVerified: {EmailVerified}",
                    payload.Subject,
                    payload.Email,
                    payload.EmailVerified);

                return new GoogleUserInfo
                {
                    Subject = payload.Subject,
                    Email = payload.Email,
                    EmailVerified = payload.EmailVerified,
                    Name = payload.Name,
                    Picture = payload.Picture
                };
            }
            catch (InvalidJwtException ex)
            {
                _logger.LogWarning("Invalid Google JWT: {Message}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error verifying Google ID token");
                return null;
            }
        }
    }
}
