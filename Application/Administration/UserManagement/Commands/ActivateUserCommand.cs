namespace Daco.Application.Administration.UserManagement.Commands
{
    public record ActivateUserCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId { get; init; }
    }
}
