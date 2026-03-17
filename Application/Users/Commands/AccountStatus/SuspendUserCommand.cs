namespace Daco.Application.Users.Commands.AccountStatus
{
    public record SuspendUserCommand : IRequest<ResponseDTO>
    {
        public Guid   UserId { get; init; }
        public string Reason { get; init; } = null!;
    }
}
