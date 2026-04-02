namespace Daco.Domain.Shops.Entities
{
    public class ShopAddress : Entity
    {
        public Guid            ShopId         { get; private set; }
        public string?         Label          { get; private set; }
        public ShopAddressType AddressType    { get; private set; }
        // Address info
        public string          ContactName    { get; private set; }
        public string          ContactPhone   { get; private set; }
        public string          City           { get; private set; }
        public string          District       { get; private set; }
        public string          Ward           { get; private set; }
        public string          AddressDetail  { get; private set; }
        // Coordinates
        public double          Latitude       { get; private set; }
        public double          Longitude      { get; private set; }
        public string?         GooglePlaceId  { get; private set; }
        // Operating hours: {"monday": {"open": "08:00", "close": "17:00"}}
        public string?         OperatingHours { get; private set; }
        public bool            IsDefault      { get; private set; }
        public bool            IsActive       { get; private set; }
        // Timestamps
        public DateTime        CreatedAt      { get; private set; }
        public DateTime?       UpdatedAt      { get; private set; }
        public DateTime?       DeletedAt      { get; private set; }

        protected ShopAddress() { }

        public static ShopAddress Create(
            Guid shopId,
            ShopAddressType addressType,
            string contactName,
            string contactPhone,
            string city,
            string district,
            string ward,
            string addressDetail,
            double latitude,
            double longitude,
            string? label = null,
            string? googlePlaceId = null,
            string? operatingHours = null,
            bool isDefault = false)
        {
            Guard.AgainstNullOrEmpty(contactName, nameof(contactName));
            Guard.AgainstNullOrEmpty(contactPhone, nameof(contactPhone));
            Guard.AgainstNullOrEmpty(city, nameof(city));
            Guard.AgainstNullOrEmpty(district, nameof(district));
            Guard.AgainstNullOrEmpty(ward, nameof(ward));
            Guard.AgainstNullOrEmpty(addressDetail, nameof(addressDetail));

            return new ShopAddress
            {
                Id = Guid.NewGuid(),
                ShopId = shopId,
                AddressType = addressType,
                ContactName = contactName,
                ContactPhone = contactPhone,
                City = city,
                District = district,
                Ward = ward,
                AddressDetail = addressDetail,
                Latitude = latitude,
                Longitude = longitude,
                Label = label,
                GooglePlaceId = googlePlaceId,
                OperatingHours = operatingHours,
                IsDefault = isDefault,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Update(
            string contactName,
            string contactPhone,
            string city,
            string district,
            string ward,
            string addressDetail,
            double latitude,
            double longitude,
            string? label = null,
            string? operatingHours = null)
        {
            Guard.AgainstNullOrEmpty(contactName, nameof(contactName));
            Guard.AgainstNullOrEmpty(contactPhone, nameof(contactPhone));
            Guard.AgainstNullOrEmpty(city, nameof(city));
            Guard.AgainstNullOrEmpty(district, nameof(district));
            Guard.AgainstNullOrEmpty(ward, nameof(ward));
            Guard.AgainstNullOrEmpty(addressDetail, nameof(addressDetail));

            ContactName = contactName;
            ContactPhone = contactPhone;
            City = city;
            District = district;
            Ward = ward;
            AddressDetail = addressDetail;
            Latitude = latitude;
            Longitude = longitude;
            Label = label ?? Label;
            OperatingHours = operatingHours ?? OperatingHours;
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

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDefault = false;
            IsActive = false;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsDeleted => DeletedAt.HasValue;

        public string GetFullAddress()
            => $"{AddressDetail}, {Ward}, {District}, {City}";
    }
}
