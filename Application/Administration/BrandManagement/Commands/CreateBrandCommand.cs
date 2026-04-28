namespace Daco.Application.Administration.BrandManagement.Commands
{
    public record CreateBrandCommand : IRequest<ResponseDTO>
    {
        public string BrandName { get; init; }
        public string BrandSlug { get; init; }
        public string? Description { get; init; }
        public string? WebsiteUrl { get; init; }
        public string? LogoUrl { get; init; }
        public string[]? SampleImages { get; init; }
    }
}
