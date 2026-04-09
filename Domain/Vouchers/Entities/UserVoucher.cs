namespace Daco.Domain.Vouchers.Entities
{
    public class UserVoucher : Entity
    {
        public Guid              UserId    { get; private set; }
        public Guid              VoucherId { get; private set; }
        public UserVoucherStatus Status    { get; private set; }
        public Guid?             OrderId   { get; private set; }
        public DateTime          ClaimedAt { get; private set; }
        public DateTime?         UsedAt    { get; private set; }
        public DateTime?         ExpiredAt { get; private set; }

        protected UserVoucher() { }
    }
}
