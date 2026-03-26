namespace Daco.Application.Administration.UserManagement.DTOs
{
    public class UserListItemDTO
    {
        public Guid      Id            { get; set; }
        public string    Username      { get; set; } = null!;
        public string?   Email         { get; set; }
        public string?   Phone         { get; set; }
        public string?   Name          { get; set; }
        public string?   Avatar        { get; set; }
        public string    Status        { get; set; } = null!;
        public bool      EmailVerified { get; set; }
        public bool      PhoneVerified { get; set; }
        public string?   Gender        { get; set; }
        public bool      IsSeller      { get; set; }
        public DateTime  CreatedAt     { get; set; }
        public DateTime? UpdatedAt     { get; set; }
    }
}
