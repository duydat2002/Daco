namespace Daco.Application.Shops.Commands
{
    public record UpdateShopBasicInfoCommand :IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid    UserId      { get; init; }
        public string? ShopName    { get; init; }
        public string? Description { get; init; }
        public string? ShopEmail   { get; init; }
        public string? ShopPhone   { get; init; }
    }
}
