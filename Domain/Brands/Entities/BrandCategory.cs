namespace Daco.Domain.Brands.Entities
{
    public class BrandCategory : Entity
    {
        public Guid     BrandId    { get; private set; }
        public Guid     CategoryId { get; private set; }
        public DateTime CreatedAt  { get; private set; }

        protected BrandCategory() { }
    }
}
