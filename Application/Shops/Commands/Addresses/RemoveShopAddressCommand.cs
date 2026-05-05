namespace Daco.Application.Shops.Commands.Addresses
{
    public record RemoveShopAddressCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid UserId    { get; init; }
        [JsonIgnore]
        public Guid AddressId { get; init; }
    }
}
