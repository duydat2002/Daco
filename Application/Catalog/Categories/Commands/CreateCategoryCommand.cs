namespace Daco.Application.Catalog.Categories.Commands
{
    public record CreateCategoryCommand : IRequest<ResponseDTO>
    {
        public string  CategoryName { get; init; } = null!;
        public Guid?   ParentId     { get; init; }
        public string? Description  { get; init; }
        public string? IconUrl      { get; init; }
        public string? ImageUrl     { get; init; }
        public int     SortOrder    { get; init; } = 0;
        [JsonIgnore]
        public Guid    CreatedBy    { get; init; }
    }
}
