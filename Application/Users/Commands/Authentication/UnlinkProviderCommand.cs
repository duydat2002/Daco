namespace Daco.Application.Users.Commands.Authentication
{
    public record UnlinkProviderCommand : IRequest<ResponseDTO>
    {
        public string ProviderType { get; init; } = null!; // "google" | "facebook"

        [JsonIgnore] 
        public Guid?  UserId       { get; init; }
    }
}
