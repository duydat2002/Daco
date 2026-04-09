namespace Daco.Domain.Shops.Entities
{
    public class ShopNotificationSetting : Entity
    {
        public Guid      ShopId                    { get; private set; }
        public bool      NotifyNewOrder            { get; private set; }
        public bool      NotifyOrderPaid           { get; private set; }
        public bool      NotifyOrderCancelled      { get; private set; }
        public bool      NotifyOrderReturned       { get; private set; }
        public bool      NotifyProductLowStock     { get; private set; }
        public bool      NotifyProductOutOfStock   { get; private set; }
        public bool      NotifyProductReview       { get; private set; }
        public bool      NotifyProductQuestion     { get; private set; }
        public bool      NotifyNewFollower         { get; private set; }
        public bool      NotifyShopViolation       { get; private set; }
        public bool      NotifyShopPenalty         { get; private set; }
        public bool      NotifyPaymentReceived     { get; private set; }
        public bool      NotifyWithdrawalCompleted { get; private set; }
        public bool      NotifyPolicyUpdate        { get; private set; }
        public bool      NotifySystemMaintenance   { get; private set; }
        public bool      EmailEnabled              { get; private set; }
        public bool      SmsEnabled                { get; private set; }
        public bool      PushEnabled               { get; private set; }
        public bool      InAppEnabled              { get; private set; }
        public bool      EnableQuietHours          { get; private set; }
        public TimeOnly? QuietHoursStart           { get; private set; }
        public TimeOnly? QuietHoursEnd             { get; private set; }
        public DateTime  CreatedAt                 { get; private set; }
        public DateTime? UpdatedAt                 { get; private set; }

        private ShopNotificationSetting() { }
    }
}
