namespace Daco.Application.Users.Commands.Authentication
{
    public record LoginWithGoogleCommand : IRequest<ResponseDTO>
    {
        public string IdToken { get; init; } = null!;
        [JsonIgnore]
        public string? IpAddress { get; init; } = null!;
        [JsonIgnore]
        public string? UserAgent { get; init; }
        [JsonIgnore]
        public string? DeviceType { get; init; }
    }
}
