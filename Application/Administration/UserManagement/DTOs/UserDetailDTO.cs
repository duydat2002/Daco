namespace Daco.Application.Administration.UserManagement.DTOs
{
    public class UserDetailDTO
    {
        public Guid      Id            { get; set; }
        public string    Username      { get; set; } = null!;
        public string?   Email         { get; set; }
        public string?   Phone         { get; set; }
        public string?   Name          { get; set; }
        public string?   Avatar        { get; set; }
        public string?   DateOfBirth   { get; set; }
        public string?   Gender        { get; set; }
        public string    Status        { get; set; } = null!;
        public bool      EmailVerified { get; set; }
        public bool      PhoneVerified { get; set; }
        public DateTime  CreatedAt     { get; set; }
        public DateTime? UpdatedAt     { get; set; }

        // Roles
        public bool IsSeller { get; set; }
        public bool IsAdmin  { get; set; }

        // Auth providers (loại bỏ sensitive fields)
        public List<UserAuthProviderDTO> AuthProviders { get; set; } = new();

        // Stats
        public int TotalOrders       { get; set; }
        public int TotalAddresses    { get; set; }
        public int TotalBankAccounts { get; set; }
    }

    public class UserAuthProviderDTO
    {
        public string    ProviderType  { get; set; } = null!;
        public bool      IsVerified    { get; set; }
        public DateTime  CreatedAt     { get; set; }
        public DateTime? VerifiedAt    { get; set; }
        public string?   ProviderEmail { get; set; }
    }
}
