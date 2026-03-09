namespace Daco.Application.Users.Commands.Verifications
{
    public record ResendOtpCommand : IRequest<ResponseDTO>
    {
        public Guid UserId { get; init; }
    }
}
