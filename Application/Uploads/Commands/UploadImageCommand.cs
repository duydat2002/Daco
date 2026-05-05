namespace Daco.Application.Uploads.Commands
{
    public class UploadImageCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid      UserId      { get; init; }
        public string    Type        { get; init; }
        [JsonIgnore]
        public Stream    FileStream  { get; init; } = null!;
        public string    FileName    { get; init; } = null!;
        public string    ContentType { get; init; } = null!;
        public long      FileSize    { get; init; }
    }
}
