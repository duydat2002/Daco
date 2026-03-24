namespace Daco.Application.Administration.AdminManagement.DTOs
{
    public class AdminDetailDTO
    {
        public Guid                Id           { get; set; }
        public Guid                UserId       { get; set; }
        public string              Username     { get; set; } = null!;
        public string?             Email        { get; set; }
        public string?             Phone        { get; set; }
        public string?             Avatar       { get; set; }
        public string?             EmployeeCode { get; set; }
        public string?             Department   { get; set; }
        public string?             Position     { get; set; }
        public string?             WorkEmail    { get; set; }
        public string?             WorkPhone    { get; set; }
        public string              Status       { get; set; } = null!;
        public string?             Notes        { get; set; }
        public DateTime            CreatedAt    { get; set; }
        public DateTime?           UpdatedAt    { get; set; }
        public DateTime?           LastLoginAt  { get; set; }
        public List<RoleDTO>       Roles        { get; set; } = new();
        public List<PermissionDTO> Permissions  { get; set; } = new();
    }

    public class RoleDTO
    {
        public Guid      Id         { get; set; }
        public string    RoleCode   { get; set; } = null!;
        public string    RoleName   { get; set; } = null!;
        public DateTime  AssignedAt { get; set; }
        public DateTime? ExpiresAt  { get; set; }
    }

    public class PermissionDTO
    {
        public string PermissionCode { get; set; } = null!;
        public string Module         { get; set; } = null!;
        public bool   IsCustom       { get; set; }  
        public bool   IsGranted      { get; set; }  
    }
}
