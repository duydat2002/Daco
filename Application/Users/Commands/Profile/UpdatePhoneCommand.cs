namespace Daco.Application.Users.Commands.Profile
{
    public record UpdatePhoneCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId { get; init; }
        public string Phone { get; init; } = null!;
    }
}
