namespace Daco.Application.Auth.Commands
{
    public record VerifyEmailCommand : IRequest<ResponseDTO>
    {
        public Guid UserId { get; init; }
        public string Otp { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress { get; init; }
        [JsonIgnore]
        public string? UserAgent { get; init; }
        [JsonIgnore]
        public string? DeviceType { get; init; }
    }
}
