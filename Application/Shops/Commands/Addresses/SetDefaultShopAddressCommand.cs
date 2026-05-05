namespace Daco.Application.Shops.Commands.Addresses
{
    public record SetDefaultShopAddressCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId    { get; init; }
        [JsonIgnore]
        public Guid AddressId { get; init; }
    }
}
