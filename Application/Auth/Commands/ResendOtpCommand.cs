namespace Daco.Application.Auth.Commands
{
    public record ResendOtpCommand : IRequest<ResponseDTO>
    {
        public Guid UserId { get; init; }
    }
}
