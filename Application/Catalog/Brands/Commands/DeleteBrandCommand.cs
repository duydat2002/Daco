namespace Daco.Application.Catalog.Brands.Commands
{
    public record DeleteBrandCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid BrandId { get; init; }
    }
}
