namespace Daco.Domain.Contents.Aggregates
{
    public class Page : AggregateRoot
    {
        public string     PageTitle       { get; private set; }
        public string     PageSlug        { get; private set; }
        public string     Content         { get; private set; } //HTML content
        public string?    Excerpt         { get; private set; } //Summary
        public string     Template        { get; private set; } //'default', 'full-width', 'with-sidebar'
        public PageStatus Status          { get; private set; }
        public bool       IsPublic        { get; private set; }
        public bool       RequireLogin    { get; private set; }
        public string?    MetaTitle       { get; private set; }
        public string?    MetaDescription { get; private set; }
        public string?    MetaKeywords    { get; private set; }
        public bool       ShowInFooter    { get; private set; }
        public int        FooterOrder     { get; private set; }
        public Guid?      AuthorId        { get; private set; }
        public DateTime   CreatedAt       { get; private set; }
        public DateTime?  UpdatedAt       { get; private set; }
        public DateTime?  PublishedAt     { get; private set; }

        protected Page() { }
    }
}
