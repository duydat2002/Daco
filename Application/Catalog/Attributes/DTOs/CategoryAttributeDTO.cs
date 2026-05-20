namespace Daco.Application.Catalog.Attributes.DTOs
{
    public class CategoryAttributeDTO
    {
        public Guid                          Id            { get; set; }
        public string                        AttributeName { get; set; } = null!;
        public string                        AttributeSlug { get; set; } = null!;
        public string                        InputType     { get; set; } = null!; 
        public bool                          IsRequired    { get; set; }
        public bool                          IsVariation   { get; set; }
        public int                           SortOrder     { get; set; }
        public string?                       Unit          { get; set; }
        public string[]?                     UnitList      { get; set; }
        public List<AttributeValueOptionDTO> Options       { get; set; } = new(); 
    }

    public class AttributeValueOptionDTO
    {
        public Guid   Id        { get; set; }
        public string Value     { get; set; } = null!;
        public int    SortOrder { get; set; }
    }
}
