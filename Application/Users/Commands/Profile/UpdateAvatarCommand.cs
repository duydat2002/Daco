namespace Daco.Application.Users.Commands.Profile
{
    public record UpdateAvatarCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   UserId      { get; init; }
        [JsonIgnore]
        public Stream FileStream  { get; init; } = null!;
        public string FileName    { get; init; } = null!;
        public string ContentType { get; init; } = null!;
        public long   FileSize    { get; init; }
    }
}
