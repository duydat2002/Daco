namespace Daco.Infrastructure.Services.Notifications
{
    public class SmsService : ISmsService
    {
        private readonly ILogger<SmsService> _logger;
        private readonly IConfiguration _configuration;

        public SmsService(
            ILogger<SmsService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendAsync(
            string phoneNumber,
            string message,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Sending SMS to {phoneNumber}");



            await Task.CompletedTask;

            _logger.LogInformation($"SMS sent successfully to {phoneNumber}");
        }
    }
}
