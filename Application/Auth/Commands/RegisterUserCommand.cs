namespace Daco.Application.Auth.Commands
{
    public record RegisterUserCommand : IRequest<ResponseDTO>
    {
        public string Username { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Phone { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
