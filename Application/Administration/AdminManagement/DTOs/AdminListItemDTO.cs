namespace Daco.Application.Administration.AdminManagement.DTOs
{
    public class AdminListItemDTO
    {
        public Guid         Id           { get; set; }
        public Guid         UserId       { get; set; }
        public string       Username     { get; set; } = null!;
        public string?      Email        { get; set; }
        public string?      EmployeeCode { get; set; }
        public string?      Department   { get; set; }
        public string?      Position     { get; set; }
        public string       Status       { get; set; } = null!;
        public DateTime     CreatedAt    { get; set; }
        public DateTime?    LastLoginAt  { get; set; }
        public List<string> Roles        { get; set; } = new();
    }
}
