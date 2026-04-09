namespace Daco.Domain.Users.Entities
{
    public class Wishlist : Entity
    {
        public Guid     UserId    { get; private set; }
        public Guid     ProductId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected Wishlist() { }
    }
}
