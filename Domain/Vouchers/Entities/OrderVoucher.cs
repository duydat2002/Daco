namespace Daco.Domain.Vouchers.Entities
{
    public class OrderVoucher : Entity
    {
        public Guid     OrderId        { get; private set; }
        public Guid     VoucherId      { get; private set; }
        public decimal  DiscountAmount { get; private set; }
        public DateTime CreatedAt      { get; private set; }

        protected OrderVoucher() { }
    }
}
