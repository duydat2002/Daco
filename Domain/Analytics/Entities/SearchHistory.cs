namespace Daco.Domain.Analytics.Entities
{
    public class SearchHistory : Entity
    {
        public Guid?    UserId           { get; private set; }
        public Guid?    SessionId        { get; private set; }
        public string   SearchQuery      { get; private set; }
        public string   SearchType       { get; private set; } //'product', 'shop', 'category'
        public string?  Filters          { get; private set; } //{"category": "...", "price_min": 100000, ...}
        public int      ResultsCount     { get; private set; }
        public Guid?    ClickedProductId { get; private set; } //Sản phẩm được click
        public int      ClickedPosition  { get; private set; } //Vị trí trong kết quả (1, 2, 3...)
        public string?  Source           { get; private set; } //'search_bar', 'voice_search', 'image_search'
        public string?  DeviceType       { get; private set; } //'mobile', 'desktop', 'tablet'
        public string?  IpAddress        { get; private set; }
        public string?  CountryCode      { get; private set; }
        public string?  City             { get; private set; }
        public DateTime CreatedAt        { get; private set; }

        protected SearchHistory() { }
    }
}
