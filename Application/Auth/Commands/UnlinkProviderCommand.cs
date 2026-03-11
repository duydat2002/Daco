namespace Daco.Application.Auth.Commands
{
    public record UnlinkProviderCommand : IRequest<ResponseDTO>
    {
        public string ProviderType { get; init; } = null!; // "google" | "facebook"

        [JsonIgnore]
        public Guid? UserId { get; init; }
    }
}
