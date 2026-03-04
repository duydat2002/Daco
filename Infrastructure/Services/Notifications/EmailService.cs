namespace Daco.Infrastructure.Services.Notifications
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;
        private readonly string _templateRoot;

        public EmailService(
            IOptions<EmailSettings> settings,
            ILogger<EmailService> logger)
        {
            _logger = logger;
            _settings = settings.Value;
            _templateRoot = Path.Combine(AppContext.BaseDirectory, "Templates", "Emails");
        }

        public Task SendOtpAsync(string to, string otp, CancellationToken cancellationToken = default)
        {
            var body = LoadTemplate("verification-otp.html")
                .Replace("{{Otp}}", otp);

            return SendAsync(to, "Verify your email - Daco", body, cancellationToken);
        }

        public Task SendWelcomeAsync(string to, string email, CancellationToken cancellationToken = default)
        {
            var body = LoadTemplate("welcome.html")
                .Replace("{{Email}}", email);

            return SendAsync(to, "Welcome to Daco!", body, cancellationToken);
        }

        public Task SendPasswordChangedAsync(string to, CancellationToken cancellationToken = default)
        {
            var body = LoadTemplate("password-changed.html")
                .Replace("{{ChangedAt}}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

            return SendAsync(to, "Security Alert - Password Changed", body, cancellationToken);
        }

        public Task SendAccountSuspendedAsync(string to, string reason, CancellationToken cancellationToken = default)
        {
            var body = LoadTemplate("account-suspended.html")
                .Replace("{{Reason}}", reason);

            return SendAsync(to, "Account Suspended - Daco", body, cancellationToken);
        }

        private string LoadTemplate(string fileName)
        {
            var path = Path.Combine(_templateRoot, fileName);
            return File.ReadAllText(path);
        }

        private async Task SendAsync(
            string to,
            string subject,
            string body,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromAddress));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = new SmtpClient();

            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_settings.Username, _settings.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);

            _logger.LogInformation("Email sent successfully to {To}", to);
        }
    }
}
