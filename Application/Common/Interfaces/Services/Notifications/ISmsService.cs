namespace Daco.Application.Common.Interfaces.Services.Notifications
{
    public interface ISmsService
    {
        Task SendAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
    }
}
