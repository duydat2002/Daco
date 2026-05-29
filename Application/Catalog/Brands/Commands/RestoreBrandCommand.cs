namespace Daco.Application.Catalog.Brands.Commands
{
    public record RestoreBrandCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid BrandId { get; init; }
    }
}
