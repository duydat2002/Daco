namespace Daco.Domain.Contents.Aggregates
{
    public class Banner : AggregateRoot
    {
        public string         BannerName     { get; private set; }
        public string?        Description    { get; private set; }
        public BannerType     BannerType     { get; private set; }
        public string?        ImageUrl       { get; private set; }
        public string?        ImageMobileUrl { get; private set; }
        public string?        VideoUrl       { get; private set; }
        public string?        HtmlContent    { get; private set; }
        public string?        LinkUrl        { get; private set; }
        public string?        LinkTarget     { get; private set; } //LinkTargets '_blank', '_self'
        public BannerPosition Position       { get; private set; }
        public int            SortOrder      { get; private set; }
        public int?           Width          { get; private set; }
        public int?           Height         { get; private set; }
        public string?        TargetAudience { get; private set; } //{"user_type": "all", "location": "VN"}
        public bool           IsActive       { get; private set; }
        public DateTime       StartDate      { get; private set; }
        public DateTime       EndDate        { get; private set; }
        public int            ViewCount      { get; private set; }
        public int            ClickCount     { get; private set; }
        public string?        AltText        { get; private set; }
        public Guid?          CreatedBy      { get; private set; }
        public DateTime       CreatedAt      { get; private set; }
        public DateTime?      UpdatedAt      { get; private set; }

        protected Banner() { }
    }
}
