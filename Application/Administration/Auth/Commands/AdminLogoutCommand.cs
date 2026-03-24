namespace Daco.Application.Administration.Auth.Commands
{
    public record AdminLogoutCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid?   UserId           { get; init; } = Guid.Empty;
        [JsonIgnore]
        public string? RawToken         { get; init; } = null!;
        public bool    LogoutAllDevices { get; init; } = false;
    }
}
