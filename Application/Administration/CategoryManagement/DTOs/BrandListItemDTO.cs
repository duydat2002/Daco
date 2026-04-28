namespace Daco.Application.Administration.CategoryManagement.DTOs
{
    public class BrandListItemDTO
    {
        public Guid     Id { get; set; }
        public string   BrandName { get; set; } = null!;
        public string   BrandSlug { get; set; } = null!;
        public string?  Description { get; set; }
        public string?  LogoUrl { get; set; }
        public bool     IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
