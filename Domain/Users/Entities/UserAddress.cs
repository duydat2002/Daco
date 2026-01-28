using Domain.Common;
using Shared.Common;

namespace Domain.Users.Entities
{
    public class UserAddress : BaseEntity
    {
        public Guid UserId { get; private set; }

        // Address info
        public string Label          { get; private set; }              
        public string AddressType    { get; private set; }        // home, office, other
        public string RecipientName  { get; private set; }
        public string RecipientPhone { get; private set; }
        public string City           { get; private set; }
        public string District       { get; private set; }
        public string Ward           { get; private set; }
        public string AddressDetail  { get; private set; }

        // Optional location
        public double? Latitude      { get; private set; }
        public double? Longitude     { get; private set; }
        public string? GooglePlaceId { get; private set; }

        // Flags
        public bool IsDefault { get; private set; }

        // Timestamps
        public DateTime  CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        private UserAddress() { }

        public static UserAddress Create(
            Guid userId,
            string recipientName,
            string recipientPhone,
            string city,
            string district,
            string ward,
            string addressDetail,
            string? label = null,
            string addressType = "home",
            double? latitude = null,
            double? longitude = null,
            bool isDefault = false
        )
        {
            Guard.AgainstNullOrEmpty(recipientName, nameof(recipientName));
            Guard.AgainstNullOrEmpty(recipientPhone, nameof(recipientPhone));
            Guard.AgainstNullOrEmpty(city, nameof(city));
            Guard.AgainstNullOrEmpty(district, nameof(district));
            Guard.AgainstNullOrEmpty(ward, nameof(ward));
            Guard.AgainstNullOrEmpty(addressDetail, nameof(addressDetail));

            return new UserAddress
            {
                UserId = userId,
                Label = label ?? "",
                AddressType = addressType,
                RecipientName = recipientName,
                RecipientPhone = recipientPhone,
                City = city,
                District = district,
                Ward = ward,
                AddressDetail = addressDetail,
                Latitude = latitude,
                Longitude = longitude,
                IsDefault = isDefault,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Update(
            string recipientName,
            string recipientPhone,
            string city,
            string district,
            string ward,
            string addressDetail,
            string? label = null,
            double? latitude = null,
            double? longitude = null
        )
        {
            Guard.AgainstNullOrEmpty(recipientName, nameof(recipientName));
            Guard.AgainstNullOrEmpty(recipientPhone, nameof(recipientPhone));
            Guard.AgainstNullOrEmpty(city, nameof(city));
            Guard.AgainstNullOrEmpty(district, nameof(district));
            Guard.AgainstNullOrEmpty(ward, nameof(ward));
            Guard.AgainstNullOrEmpty(addressDetail, nameof(addressDetail));

            RecipientName = recipientName;
            RecipientPhone = recipientPhone;
            City = city;
            District = district;
            Ward = ward;
            AddressDetail = addressDetail;
            Label = label ?? Label;
            Latitude = latitude;
            Longitude = longitude;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAsDefault()
        {
            IsDefault = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveDefault()
        {
            IsDefault = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public string GetFullAddress()
        {
            return $"{AddressDetail}, {Ward}, {District}, {City}";
        }

        public bool IsDeleted => DeletedAt.HasValue;
    }
}
