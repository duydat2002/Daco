namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public record UpdateCategoryCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid    CategoryId   { get; init; }
        public Guid?   ParentId     { get; init; }
        public string  CategoryName { get; init; }
        public string  CategorySlug { get; init; }
        public string? Description  { get; init; }
        public string? IconUrl      { get; init; }
        public string? ImageUrl     { get; init; }
        public int     SortOrder    { get; init; } = 0;
        public bool    IsActive     { get; init; } = true;
        [JsonIgnore]
        public Guid    UpdatedBy    { get; init; }
    }
}
