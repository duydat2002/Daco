using Domain.Common;
using Domain.Users.Enums;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities
{
    public class User : BaseEntity
    {
        public string       UserName      { get; private set; }
        public Email?       Email         { get; private set; }
        public PhoneNumber? Phone         { get; private set; }
        public string?      Name          { get; private set; }
        public string?      Avatar        { get; private set; }
        public int?         DateOfBirth   { get; private set; }
        public UserStatus   Status        { get; private set; }
        public bool         EmailVerified { get; private set; }
        public bool         PhoneVerified { get; private set; }
        public DateTime     CreatedAt     { get; private set; }
        public DateTime     UpdatedAt     { get; private set; }
        public DateTime     DeletedAt     { get; private set; }

        private User() { }

        private User(string userName, Email? email, PhoneNumber? phone, string name)
        {
            if (email is null && phone is null)
                throw new InvalidOperationException("User must have email or phone");

            UserName = userName;
            Email = email;
            Phone = phone;
            Name = name;
        }   

        public void Activate()
        {
            Status = UserStatus.Active;
        }
    }
}
