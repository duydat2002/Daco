namespace Daco.Application.Users.Commands.Address
{
    public record SetDefaultAddressCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId    { get; init; }
        [JsonIgnore]
        public Guid AddressId { get; init; }
    }
}
