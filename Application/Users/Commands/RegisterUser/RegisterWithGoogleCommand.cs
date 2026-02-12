namespace Daco.Application.Users.Commands.RegisterUser
{
    public record RegisterWithGoogleCommand : IRequest<ResponseDTO>
    {
        public string IdToken { get; init; } = null!;
    }
}
