namespace Daco.Application.Administration.CategoryManagement.Commands
{
    public record CreateCategoryAttributeCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid               CategoryId        { get; init; }
        [JsonIgnore]
        public Guid               CreatedBy         { get; init; }
        public string             AttributeName     { get; init; } = null!;
        public string             AttributeSlug     { get; init; } = null!;
        public string?            Description       { get; init; }
        public AttributeInputType InputType         { get; init; }
        public bool               IsRequired        { get; init; } = false;
        public bool               IsVariation       { get; init; } = false;
        public int                SortOrder         { get; init; } = 0;
        public string?            Unit              { get; init; }
        public string[]?          AttributeUnitList { get; init; }
        public List<string>?      PredefinedValues  { get; init; }
    }
}
