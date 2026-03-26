namespace Daco.Domain.Orders.ValueObjects
{
    public sealed class ShippingAddress : ValueObject
    {
        public string RecipientName  { get; }
        public string RecipientPhone { get; }
        public string City           { get; }
        public string District       { get; }
        public string Ward           { get; }
        public string AddressDetail  { get; }

        private ShippingAddress(
            string recipientName,
            string recipientPhone,
            string city,
            string district,
            string ward,
            string addressDetail)
        {
            RecipientName = recipientName;
            RecipientPhone = recipientPhone;
            City = city;
            District = district;
            Ward = ward;
            AddressDetail = addressDetail;
        }

        public static ShippingAddress Create(
            string recipientName,
            string recipientPhone,
            string city,
            string district,
            string ward,
            string addressDetail)
        {
            Guard.AgainstNullOrEmpty(recipientName, nameof(recipientName));
            Guard.AgainstNullOrEmpty(recipientPhone, nameof(recipientPhone));
            Guard.AgainstNullOrEmpty(city, nameof(city));
            Guard.AgainstNullOrEmpty(district, nameof(district));
            Guard.AgainstNullOrEmpty(ward, nameof(ward));
            Guard.AgainstNullOrEmpty(addressDetail, nameof(addressDetail));

            return new ShippingAddress(recipientName, recipientPhone, city, district, ward, addressDetail);
        }

        public string GetFullAddress()
            => $"{AddressDetail}, {Ward}, {District}, {City}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RecipientName;
            yield return RecipientPhone;
            yield return City;
            yield return District;
            yield return Ward;
            yield return AddressDetail;
        }

        public override string ToString() => GetFullAddress();
    }
}
