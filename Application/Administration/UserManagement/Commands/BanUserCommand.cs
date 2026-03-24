namespace Daco.Application.Administration.UserManagement.Commands
{
    public record BanUserCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid   UserId { get; init; }
        public string Reason { get; init; } = null!;
    }
}
