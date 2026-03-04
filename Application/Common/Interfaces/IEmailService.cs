namespace Daco.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpAsync(string to, string otp, CancellationToken cancellationToken = default);
        Task SendWelcomeAsync(string to, string email, CancellationToken cancellationToken = default);
        Task SendPasswordChangedAsync(string to, CancellationToken cancellationToken = default);
        Task SendAccountSuspendedAsync(string to, string reason, CancellationToken cancellationToken = default);

    }
}
