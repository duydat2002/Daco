namespace Daco.Domain.Carts.Aggregates
{
    public class Cart : AggregateRoot
    {
        private readonly List<CartItem> _cartItems = new();

        public Guid     UserId    { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

        protected Cart() { }
    }
}
