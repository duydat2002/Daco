namespace Daco.Application.Users.Commands.Profile
{
    public record UpdateUserProfileCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid       UserId      { get; init; }
        public string?    Name        { get; init; }
        public string?    DateOfBirth { get; init; }
        public UserGender Gender      { get; init; }
    }
}
