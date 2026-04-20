namespace Daco.Domain.Shops.Aggregates
{
    public class Shop : AggregateRoot
    {
        private readonly List<ShopAddress> _shopAddresses = new();

        public Guid        SellerId    { get; private set; }
        public string      ShopName    { get; private set; }
        public string      ShopSlug    { get; private set; }
        public string?     ShopLogo    { get; private set; }
        public string?     ShopCover   { get; private set; }
        public string?     Description { get; private set; }
        public string?     ShopEmail   { get; private set; }
        public string?     ShopPhone   { get; private set; }
        public ShopStatus  Status  { get; private set; }
        public ShopType    ShopType    { get; private set; }
        public bool        IsOfficial  { get; private set; }
        public DateTime    JoinedAt    { get; private set; }
        public DateTime    CreatedAt   { get; private set; }
        public DateTime?   UpdatedAt   { get; private set; }
        public DateTime?   DeletedAt   { get; private set; }

        public IReadOnlyCollection<ShopAddress> ShopAddresses => _shopAddresses.AsReadOnly();
        public ShopMetrics ShopMetrics     { get; private set; } = null!;
        public ShopChatSetting ShopChatSetting { get; private set; }
        public ShopNotificationSetting ShopNotificationSetting { get; private set; }

        protected Shop() { }

        public static Shop Create(
            Guid sellerId,
            string shopName,
            string shopSlug,
            string? description = null,
            string? shopEmail = null,
            string? shopPhone = null)
        {
            Guard.Against(sellerId == Guid.Empty, "SellerId is required");
            Guard.AgainstNullOrEmpty(shopName, nameof(shopName));
            Guard.AgainstNullOrEmpty(shopSlug, nameof(shopSlug));

            var shop = new Shop
            {
                Id = Guid.NewGuid(),
                SellerId = sellerId,
                ShopName = shopName.Trim(),
                ShopSlug = shopSlug.Trim().ToLowerInvariant(),
                Description = description,
                ShopEmail = shopEmail,
                ShopPhone = shopPhone,
                Status = ShopStatus.Active,
                ShopType = ShopType.Normal,
                IsOfficial = false,
                JoinedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            shop.ShopMetrics = ShopMetrics.CreateEmpty(shop.Id);

            shop.AddDomainEvent(new ShopCreatedEvent(shop.Id, sellerId));

            return shop;
        }

        public void UpdateProfile(
            string shopName,
            string? description = null,
            string? shopEmail = null,
            string? shopPhone = null)
        {
            Guard.Against(Status == ShopStatus.Closed, "Cannot update a closed shop");
            Guard.AgainstNullOrEmpty(shopName, nameof(shopName));

            ShopName = shopName.Trim();
            Description = description;
            ShopEmail = shopEmail;
            ShopPhone = shopPhone;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateLogo(string logoUrl)
        {
            Guard.AgainstNullOrEmpty(logoUrl, nameof(logoUrl));
            Guard.Against(Status == ShopStatus.Closed, "Cannot update a closed shop");

            ShopLogo = logoUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateCover(string coverUrl)
        {
            Guard.AgainstNullOrEmpty(coverUrl, nameof(coverUrl));
            Guard.Against(Status == ShopStatus.Closed, "Cannot update a closed shop");

            ShopCover = coverUrl;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddAddress(ShopAddress address)
        {
            Guard.AgainstNull(address, nameof(address));

            var activeOfType = _shopAddresses
                .Where(a => !a.IsDeleted && a.AddressType == address.AddressType)
                .ToList();

            Guard.Against(activeOfType.Count >= 5,
                $"Cannot have more than 5 active {address.AddressType} addresses");

            // Nếu chưa có address nào cùng loại thì tự đặt default
            if (!activeOfType.Any())
            {
                address.SetAsDefault();
            }
            else if (address.IsDefault)
            {
                foreach (var existing in activeOfType.Where(a => a.IsDefault))
                    existing.RemoveDefault();
            }

            _shopAddresses.Add(address);
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAddress(Guid addressId, ShopAddress updated)
        {
            var address = GetActiveAddress(addressId);

            address.Update(
                updated.ContactName,
                updated.ContactPhone,
                updated.City,
                updated.District,
                updated.Ward,
                updated.AddressDetail,
                updated.Latitude,
                updated.Longitude,
                updated.Label);

            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDefaultAddress(Guid addressId)
        {
            var address = GetActiveAddress(addressId);

            foreach (var a in _shopAddresses.Where(a => !a.IsDeleted && a.AddressType == address.AddressType && a.IsDefault))
                a.RemoveDefault();

            address.SetAsDefault();
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAddress(Guid addressId)
        {
            var address = GetActiveAddress(addressId);

            var wasDefault = address.IsDefault;
            address.SoftDelete();

            // Tự động set default cho address tiếp theo cùng loại
            if (wasDefault)
            {
                var next = _shopAddresses
                    .FirstOrDefault(a => !a.IsDeleted && a.AddressType == address.AddressType);
                next?.SetAsDefault();
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void Suspend(string reason)
        {
            Guard.Against(Status == ShopStatus.Suspended, "Shop is already suspended");
            Guard.Against(Status == ShopStatus.Closed, "Cannot suspend a closed shop");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            Status = ShopStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new ShopSuspendedEvent(Id, SellerId, reason));
        }

        public void Reinstate()
        {
            Guard.Against(Status != ShopStatus.Suspended, "Only suspended shops can be reinstated");

            Status = ShopStatus.Active;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new ShopReinstatedEvent(Id, SellerId));
        }

        public void Close(string reason)
        {
            Guard.Against(Status == ShopStatus.Closed, "Shop is already closed");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            Status = ShopStatus.Closed;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new ShopClosedEvent(Id, SellerId, reason));
        }

        public void MarkAsOfficial()
        {
            Guard.Against(Status != ShopStatus.Active, "Shop must be active to be marked as official");

            IsOfficial = true;
            ShopType = ShopType.Official;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsActive => Status == ShopStatus.Active;

        public ShopAddress? GetDefaultAddress(ShopAddressType type)
            => _shopAddresses.FirstOrDefault(a => !a.IsDeleted && a.AddressType == type && a.IsDefault);

        private ShopAddress GetActiveAddress(Guid addressId)
        {
            var address = _shopAddresses.FirstOrDefault(a => a.Id == addressId && !a.IsDeleted);
            Guard.Against(address is null, "Address not found");
            return address!;
        }
    }
}
