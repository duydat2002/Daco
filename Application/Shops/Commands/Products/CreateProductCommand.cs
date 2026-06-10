namespace Daco.Application.Shops.Commands.Products
{
    public record CreateProductCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid                     UserId           { get; init; }
        public string                   ProductName      { get; init; } = null!;
        public string?                  Description      { get; init; }
        public Guid                     CategoryId       { get; init; }
        public Guid?                    BrandId          { get; init; }
        public decimal?                 BasePrice        { get; init; }
        public decimal?                 CompareAtPrice   { get; init; }
        public int                      StockQuantity    { get; init; } = 0;
        public string?                  Sku              { get; init; }
        public string?                  Gtin             { get; init; }
        public decimal                  Weight           { get; init; }
        public decimal                  Length           { get; init; }
        public decimal                  Width            { get; init; }
        public decimal                  Height           { get; init; }
        public bool                     IsPreOrder       { get; init; } = false;
        public int                      PreOrderLeadTime { get; init; } = 0;
        public List<ProductImageInput>  Images           { get; init; } = new();
        public ProductVideoInput?       Video            { get; init; } = new();
    }

    public record ProductImageInput
    {
        public Guid?  Id        { get; init; }
        public string TempUrl   { get; init; } = null!; 
        public int    SortOrder { get; init; }
    }

    public record ProductVideoInput
    {
        public string TempUrl      { get; init; } = null!;
        public string TempThumbUrl { get; init; } = null!;
    }
}
