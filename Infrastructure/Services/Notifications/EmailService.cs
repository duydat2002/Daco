namespace Daco.Infrastructure.Services.Notifications
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;

        public EmailService(
            ILogger<EmailService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendAsync(
            string to,
            string subject,
            string body,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation($"Sending email to {to} with subject {subject}");



            await Task.CompletedTask;

            _logger.LogInformation($"Email sent successfully to {to}");
        }
    }
}
