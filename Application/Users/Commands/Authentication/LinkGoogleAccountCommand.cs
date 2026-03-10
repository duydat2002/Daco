namespace Daco.Application.Users.Commands.Authentication
{
    public record LinkGoogleAccountCommand : IRequest<ResponseDTO>
    {
        public string IdToken { get; init; } = null!;
        [JsonIgnore]
        public Guid?  UserId { get; init; } = Guid.Empty;
    }
}
