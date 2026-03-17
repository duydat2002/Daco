namespace Daco.Application.Users.Commands.Profile
{
    public record UpdateEmailCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId { get; init; }
        public string Email { get; init; } = null!;
    }
}
