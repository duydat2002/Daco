namespace Daco.Application.Catalog.Brands.Commands
{
    public record CreateBrandCommand : IRequest<ResponseDTO>
    {
        public string       BrandName    { get; init; }
        public string?      Description  { get; init; }
        public string?      WebsiteUrl   { get; init; }
        public string?      LogoUrl      { get; init; }
        public List<string> SampleImages { get; init; }
    }
}
