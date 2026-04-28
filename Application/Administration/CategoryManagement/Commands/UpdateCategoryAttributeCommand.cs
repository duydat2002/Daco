namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public record UpdateCategoryAttributeCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid               AttributeId       { get; init; }
        public string             AttributeName     { get; init; } = null!;
        public string             AttributeSlug     { get; init; } = null!;
        public AttributeInputType InputType         { get; init; }
        public bool               IsRequired        { get; init; }
        public bool               IsVariation       { get; init; }
        public int                SortOrder         { get; init; } = 0;
        public string?            Description       { get; init; }
        public string?            Unit              { get; init; }
        public string[]?          AttributeUnitList { get; init; }
        public List<string>?      PredefinedValues  { get; init; }  // null = không update, empty = xóa hết
    }
}
