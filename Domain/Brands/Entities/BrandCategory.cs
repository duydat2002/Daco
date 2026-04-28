namespace Daco.Domain.Brands.Entities
{
    public class BrandCategory : Entity
    {
        public Guid     BrandId    { get; private set; }
        public Guid     CategoryId { get; private set; }
        public DateTime CreatedAt  { get; private set; }

        protected BrandCategory() { }

        public static BrandCategory Create(Guid brandId, Guid categoryId)
        {
            Guard.Against(brandId == Guid.Empty, "BrandId is required");
            Guard.Against(categoryId == Guid.Empty, "CategoryId is required");

            return new BrandCategory
            {
                Id = Guid.NewGuid(),
                BrandId = brandId,
                CategoryId = categoryId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
