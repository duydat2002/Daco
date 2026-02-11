namespace Daco.Infrastructure.Persistence.Models.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? Avatar { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Gender { get; set; }
        public int Status { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
