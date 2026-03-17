namespace Daco.Application.Users.Commands.Profile
{
    public record UpdateUsernameCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   UserId  { get; init; }
        public string Username { get; init; } = null!;
    }
}
