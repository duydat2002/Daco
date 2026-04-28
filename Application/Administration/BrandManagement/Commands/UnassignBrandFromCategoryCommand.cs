namespace Daco.Application.Administration.BrandManagement.Commands
{
    public record UnassignBrandFromCategoryCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid BrandId { get; init; }
        public List<Guid> CategoryIds { get; init; } = new();
    }
}
