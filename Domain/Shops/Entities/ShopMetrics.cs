namespace Daco.Domain.Shops.Entities
{
    public class ShopMetrics : Entity
    {
        public Guid ShopId { get; private set; }

        public int     FollowerCount     { get; private set; }
        public int     TotalProducts     { get; private set; }
        public int     TotalOrders       { get; private set; }
        public int     TotalSold         { get; private set; }
        public decimal TotalRevenue      { get; private set; }
        // Rating
        public decimal RatingAverage     { get; private set; }
        public int     TotalRatings      { get; private set; }
        // Response
        public decimal ResponseRate      { get; private set; }  
        public decimal ResponseTime      { get; private set; }  
        public int?    ResponseTimeHours { get; private set; }

        protected ShopMetrics() { }

        public static ShopMetrics CreateEmpty(Guid shopId)
        {
            return new ShopMetrics
            {
                Id = Guid.NewGuid(),
                ShopId = shopId,
                FollowerCount = 0,
                TotalProducts = 0,
                TotalOrders = 0,
                TotalSold = 0,
                TotalRevenue = 0,
                RatingAverage = 0,
                TotalRatings = 0,
                ResponseRate = 0,
                ResponseTime = 0
            };
        }

        public void IncrementFollowers() => FollowerCount++;
        public void DecrementFollowers() => FollowerCount = Math.Max(0, FollowerCount - 1);

        public void IncrementProducts() => TotalProducts++;
        public void DecrementProducts() => TotalProducts = Math.Max(0, TotalProducts - 1);

        public void RecordOrder(decimal revenue)
        {
            TotalOrders++;
            TotalRevenue += revenue;
        }

        public void RecordSold(int quantity)
        {
            TotalSold += quantity;
        }

        public void UpdateRating(decimal newAverage, int totalRatings)
        {
            Guard.Against(newAverage < 0 || newAverage > 5, "Rating must be between 0 and 5");
            RatingAverage = newAverage;
            TotalRatings = totalRatings;
        }

        public void UpdateResponseStats(decimal responseRate, decimal responseTime)
        {
            Guard.Against(responseRate < 0 || responseRate > 100, "Response rate must be 0-100");
            ResponseRate = responseRate;
            ResponseTime = responseTime;
        }
    }
}
