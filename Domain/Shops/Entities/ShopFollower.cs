namespace Daco.Domain.Shops.Entities
{
    public class ShopFollower : Entity
    {
        public Guid      ShopId            { get; private set; }
        public Guid      UserId            { get; private set; }
        public bool      NotifyNewProducts { get; private set; }
        public bool      NotifyPromotions  { get; private set; }
        public bool      NotifyFlashSales  { get; private set; }
        public bool      NotifyLivestream  { get; private set; }
        public DateTime  FollowedAt        { get; private set; }
        public DateTime? UnfollowedAt      { get; private set; }

        protected ShopFollower() { }
    }
}
