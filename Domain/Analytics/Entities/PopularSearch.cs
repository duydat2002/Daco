namespace Daco.Domain.Analytics.Entities
{
    public class PopularSearch : Entity
    {
        public string   SearchQuery     { get; private set; }
        public int      SearchCount     { get; private set; }
        public int      ClickCount      { get; private set; }
        public int      ConversionCount { get; private set; } //Số lần dẫn đến mua hàng
        public string   PeriodType      { get; private set; } //'daily', 'weekly', 'monthly'
        public DateTime PeriodStart     { get; private set; }
        public DateTime PeriodEnd       { get; private set; }
        public int      Rank            { get; private set; }
        public DateTime LastUpdated     { get; private set; }

        protected PopularSearch() { }
    }
}
