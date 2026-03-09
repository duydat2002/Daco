namespace Daco.Application.Users.Commands.Authentication
{
    public record RefreshTokenCommand : IRequest<ResponseDTO>
    {
        public string  RefreshToken { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress    { get; init; }
        [JsonIgnore]
        public string? UserAgent    { get; init; }
        [JsonIgnore]
        public string? DeviceType   { get; init; }
    }
}
