namespace Daco.Domain.Vouchers.Aggregates
{
    public class Voucher : AggregateRoot
    {
        private readonly List<UserVoucher> _userVouchers = new();
        private readonly List<OrderVoucher> _orderVouchers = new();

        public string       VoucherCode          { get; private set; }
        public VoucherType  VoucherType          { get; private set; }
        public Guid?        ShopId               { get; private set; }
        public DiscountType DiscountType         { get; private set; }
        public decimal      DiscountValue        { get; private set; } //% hoặc số tiền
        public decimal?     MaxDiscountAmount    { get; private set; } //Giảm tối đa (cho %)
        public decimal      MinOrderValue        { get; private set; }
        public string?      ApplicableCategories { get; private set; } //Array of category IDs
        public string?      ApplicableProducts   { get; private set; } //Array of product IDs
        public int?         TotalQuantity        { get; private set; } //Tổng số lượng
        public int          UsedQuantity         { get; private set; }
        public int          MaxUsagePerUser      { get; private set; }
        public DateTime     StartDate            { get; private set; }
        public DateTime     EndDate              { get; private set; }
        public bool         IsActive             { get; private set; }
        public DateTime     CreatedAt            { get; private set; }
        public DateTime?    UpdatedAt            { get; private set; }

        public IReadOnlyCollection<UserVoucher> UserVouchers => _userVouchers.AsReadOnly();
        public IReadOnlyCollection<OrderVoucher> OrderVouchers => _orderVouchers.AsReadOnly();

        protected Voucher() { }
    }
}
