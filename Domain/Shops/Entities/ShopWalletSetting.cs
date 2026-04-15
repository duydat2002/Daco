namespace Daco.Domain.Shops.Entities
{
    public class ShopWalletSetting : Entity
    {
        public Guid      ShopId                  { get; private set; }
        public string?   TransactionPinHash      { get; private set; }
        public bool      PinEnabled              { get; private set; }
        public DateTime? PinUpdatedAt            { get; private set; }
        public bool      AutoWithdrawalEnabled   { get; private set; }
        public string    WithdrawalFrequency     { get; private set; }
        public int?      WithdrawalDayOfMonth    { get; private set; }
        public int[]?    WithdrawalDaysOfMonth   { get; private set; }
        public decimal   MinWithdrawalAmount     { get; private set; }
        public int       HoldPeriodDays          { get; private set; }
        public bool      NotifyWithdrawalSuccess { get; private set; }
        public bool      NotifyLowBalance        { get; private set; }
        public decimal   LowBalanceThreshold     { get; private set; }
        public DateTime  CreatedAt               { get; private set; }
        public DateTime? UpdatedAt               { get; private set; }

        protected ShopWalletSetting() { }
    }
}
