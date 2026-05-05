namespace Daco.Application.Shops.Commands.Addresses
{
    public record UpdateShopAddressCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid            UserId         { get; init; }
        [JsonIgnore]
        public Guid            AddressId      { get; init; }
        public ShopAddressType AddressType    { get; init; }
        public string          ContactName    { get; init; } = null!;
        public string          ContactPhone   { get; init; } = null!;
        public string          City           { get; init; } = null!;
        public string          District       { get; init; } = null!;
        public string          Ward           { get; init; } = null!;
        public string          AddressDetail  { get; init; } = null!;
        public double          Latitude       { get; init; }
        public double          Longitude      { get; init; }
        public string?         Label          { get; init; }
        public string?         OperatingHours { get; init; }
    }
}
