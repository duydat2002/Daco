namespace Daco.Application.Auth.Commands
{
    public record RefreshTokenCommand : IRequest<ResponseDTO>
    {
        public string RefreshToken { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress { get; init; }
        [JsonIgnore]
        public string? UserAgent { get; init; }
        [JsonIgnore]
        public string? DeviceType { get; init; }
    }
}
