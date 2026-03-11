namespace Daco.Application.Auth.Commands
{
    public record LinkGoogleAccountCommand : IRequest<ResponseDTO>
    {
        public string IdToken { get; init; } = null!;
        [JsonIgnore]
        public Guid? UserId { get; init; } = Guid.Empty;
    }
}
