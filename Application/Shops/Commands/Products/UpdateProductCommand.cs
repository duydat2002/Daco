namespace Daco.Application.Shops.Commands.Products
{
    public record UpdateProductCommand : IRequest<ResponseDTO>
    {
        [JsonIgnore]
        public Guid                    UserId           { get; init; }
        [JsonIgnore]
        public Guid                    ProductId        { get; init; }
        public string                  ProductName      { get; init; } = null!;
        public string                  ProductSlug      { get; init; } = null!;
        public string?                 Description      { get; init; }
        public Guid                    CategoryId       { get; init; }
        public Guid?                   BrandId          { get; init; }
        public decimal?                BasePrice        { get; init; }
        public decimal?                CompareAtPrice   { get; init; }
        public int                     StockQuantity    { get; init; } = 0;
        public string?                 Sku              { get; init; }
        public string?                 Gtin             { get; init; }
        public decimal                 Weight           { get; init; }
        public decimal                 Length           { get; init; }
        public decimal                 Width            { get; init; }
        public decimal                 Height           { get; init; }
        public bool                    IsPreOrder       { get; init; } = false;
        public int                     PreOrderLeadTime { get; init; } = 0;
        public string?                 MetaTitle        { get; init; }
        public string?                 MetaDescription  { get; init; }
        public string?                 MetaKeywords     { get; init; }
        public List<ProductImageInput> Images           { get; init; } = new();
        public ProductVideoInput?      Video            { get; init; } = new();
    }
}
