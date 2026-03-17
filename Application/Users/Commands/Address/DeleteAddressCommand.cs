namespace Daco.Application.Users.Commands.Address
{
    public record DeleteAddressCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId    { get; init; }
        [JsonIgnore]
        public Guid AddressId { get; init; }
    }
}
