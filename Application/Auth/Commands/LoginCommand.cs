namespace Daco.Application.Auth.Commands
{
    public record LoginCommand : IRequest<ResponseDTO>
    {
        public string Identifier { get; init; } = null!; // email hoặc phone
        public string Password { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress { get; init; } = null!;
        [JsonIgnore]
        public string? UserAgent { get; init; }
        [JsonIgnore]
        public string? DeviceType { get; init; }
    }
}
