namespace Daco.Application.Users.Commands.Verifications
{
    public record VerifyEmailCommand : IRequest<ResponseDTO>
    {
        public Guid    UserId     { get; init; }
        public string  Otp        { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress  { get; init; } = null!;
        [JsonIgnore]
        public string? UserAgent  { get; init; }
        [JsonIgnore]
        public string? DeviceType { get; init; }
    }
}
