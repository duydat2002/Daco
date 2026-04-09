namespace Daco.Domain.Shops.Entities
{
    public class ShopChatSetting : Entity
    {
        public Guid             ShopId                   { get; private set; }
        public bool             EnablePopupNotifications { get; private set; }
        public bool             EnableSoundNotifications { get; private set; }
        public string           NotificationSound        { get; private set; }
        public bool             EnableAutoReply          { get; private set; }
        public string?          AutoReplyMessage         { get; private set; }
        public int              AutoReplyDelaySeconds    { get; private set; }
        public List<QuickReply> QuickReplies             { get; private set; } = new();
        public string           EnableWorkingHours       { get; private set; }
        public WorkingHours?    WorkingHours             { get; private set; }
        public string?          OutsideHoursMessage      { get; private set; }
        public bool             AwayMode                 { get; private set; }
        public string?          AwayMessage              { get; private set; }
        public DateTime         CreatedAt                { get; private set; }
        public DateTime?        UpdatedAt                { get; private set; }

        protected ShopChatSetting() { }
    }
}
