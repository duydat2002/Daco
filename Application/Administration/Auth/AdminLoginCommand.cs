namespace Daco.Application.Administration.Auth
{
    public record AdminLoginCommand : IRequest<ResponseDTO>
    {
        public string  Identifier { get; init; } = null!; // email hoặc username
        public string  Password   { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress  { get; init; } = null!;
        [JsonIgnore]
        public string? UserAgent  { get; init; }
        [JsonIgnore]
        public string? DeviceType { get; init; }
    }
}
