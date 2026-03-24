namespace Daco.Application.Administration.Auth.Commands
{
    public record AdminVerifyOtpCommand : IRequest<ResponseDTO>
    {
        public string  TempToken  { get; init; } = null!;
        public string  Otp        { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress  { get; init; }
        [JsonIgnore]
        public string? UserAgent  { get; init; }
        [JsonIgnore]
        public string? DeviceType { get; init; }
    }
}
