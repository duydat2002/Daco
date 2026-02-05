namespace Daco.Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand : IRequest<ResponseDTO>
    {
        public string  Username { get; init; } = null!;
        public string? Email    { get; init; }
        public string? Phone    { get; init; }
        public string  Password { get; init; } = null!;
    }
}
