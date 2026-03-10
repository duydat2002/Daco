namespace Daco.Application.Users.Commands.Authentication
{
    public record LogoutCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid?   UserId           { get; init; } = Guid.Empty;  // Lấy từ JWT claims
        [JsonIgnore]
        public string? RawToken         { get; init; } = null!; 
        public bool    LogoutAllDevices { get; init; } = false;
    }
}
