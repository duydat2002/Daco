using Domain.Common;
using Domain.Users.Enums;
using Domain.Users.ValueObjects;
using Shared.Common;

namespace Domain.Users.Entities
{
    public class User : BaseEntity
    {
        private readonly List<AuthProvider> _authProviders = new();
        private readonly List<UserAddress> _addresses = new();
        private readonly List<BankAccount> _bankAccounts = new();
        private readonly List<LoginSession> _loginSessions = new();

        public Username Username { get; private set; }
        public Email? Email { get; private set; }
        public PhoneNumber? Phone { get; private set; }
        public string? Name { get; private set; }
        public string? Avatar { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public UserGender? Gender { get; private set; }
        public UserStatus Status { get; private set; }
        public bool EmailVerified { get; private set; }
        public bool PhoneVerified { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        // Navigation Properties (Entities - Read-only collections)
        public IReadOnlyCollection<AuthProvider> AuthProviders => _authProviders.AsReadOnly();
        public IReadOnlyCollection<UserAddress> Addresses => _addresses.AsReadOnly();
        public IReadOnlyCollection<BankAccount> BankAccounts => _bankAccounts.AsReadOnly();
        public IReadOnlyCollection<LoginSession> LoginSessions => _loginSessions.AsReadOnly();


        private User() { }

        /// <summary>
        /// Thêm địa chỉ mới
        /// </summary>
        public void AddAddress(
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
            bool isDefault = false)
        {
            Guard.Against(Status != UserStatus.Active, "Cannot add address for inactive user");
            Guard.Against(_addresses.Count(a => !a.IsDeleted) >= 10,
                "Cannot have more than 10 active addresses");

            // Nếu đây là địa chỉ đầu tiên hoặc được đánh dấu default
            if (!_addresses.Any(a => !a.IsDeleted) || isDefault)
            {
                // Remove default từ tất cả addresses khác
                foreach (var addr in _addresses.Where(a => !a.IsDeleted))
                {
                    addr.RemoveDefault();
                }
                isDefault = true;
            }

            var address = UserAddress.Create(
                Id,
                recipientName,
                recipientPhone,
                city,
                district,
                ward,
                addressDetail,
                label,
                addressType,
                latitude,
                longitude,
                isDefault);

            _addresses.Add(address);
            UpdatedAt = DateTime.UtcNow;

            //AddDomainEvent(new AddressAddedEvent(Id, address.Id));
        }

        /// <summary>
        /// Cập nhật địa chỉ
        /// </summary>
        public void UpdateAddress(
            Guid addressId,
            string recipientName,
            string recipientPhone,
            string city,
            string district,
            string ward,
            string addressDetail,
            string? label = null,
            double? latitude = null,
            double? longitude = null)
        {
            Guard.Against(Status != UserStatus.Active, "Cannot update address for inactive user");

            var address = _addresses.FirstOrDefault(a => a.Id == addressId && !a.IsDeleted);
            Guard.Against(address == null, "Address not found");

            address!.Update(
                recipientName,
                recipientPhone,
                city,
                district,
                ward,
                addressDetail,
                label,
                latitude,
                longitude);

            UpdatedAt = DateTime.UtcNow;

            //AddDomainEvent(new AddressUpdatedEvent(Id, addressId));
        }

        /// <summary>
        /// Xóa địa chỉ (soft delete)
        /// </summary>
        public void RemoveAddress(Guid addressId)
        {
            Guard.Against(Status != UserStatus.Active, "Cannot remove address for inactive user");

            var address = _addresses.FirstOrDefault(a => a.Id == addressId && !a.IsDeleted);
            Guard.Against(address == null, "Address not found");

            var wasDefault = address!.IsDefault;
            address.SoftDelete();

            // Nếu xóa địa chỉ default, set địa chỉ khác làm default
            if (wasDefault)
            {
                var nextDefault = _addresses.FirstOrDefault(a => !a.IsDeleted);
                nextDefault?.SetAsDefault();
            }

            UpdatedAt = DateTime.UtcNow;

            //AddDomainEvent(new AddressRemovedEvent(Id, addressId));
        }

        /// <summary>
        /// Đặt địa chỉ làm mặc định
        /// </summary>
        public void SetDefaultAddress(Guid addressId)
        {
            var address = _addresses.FirstOrDefault(a => a.Id == addressId && !a.IsDeleted);
            Guard.Against(address == null, "Address not found");

            // Remove default từ tất cả addresses
            foreach (var addr in _addresses.Where(a => !a.IsDeleted))
            {
                addr.RemoveDefault();
            }

            address!.SetAsDefault();
            UpdatedAt = DateTime.UtcNow;

            //AddDomainEvent(new DefaultAddressChangedEvent(Id, addressId));
        }

        /// <summary>
        /// Lấy địa chỉ mặc định
        /// </summary>
        public UserAddress? GetDefaultAddress()
        {
            return _addresses.FirstOrDefault(a => a.IsDefault && !a.IsDeleted);
        }

        /// <summary>
        /// Lấy tất cả địa chỉ active
        /// </summary>
        public IEnumerable<UserAddress> GetActiveAddresses()
        {
            return _addresses.Where(a => !a.IsDeleted);
        }

        // ========================================================================
        // BANK ACCOUNT MANAGEMENT (Corrected)
        // ========================================================================

        public void AddBankAccount(
            string bankCode,
            string bankName,
            string accountNumber,
            string accountHolder,
            bool isDefault = false)
        {
            Guard.Against(Status != UserStatus.Active, "Cannot add bank account for inactive user");
            Guard.Against(_bankAccounts.Count(b => !b.IsDeleted) >= 5,
                "Cannot have more than 5 active bank accounts");

            // Check duplicate
            var isDuplicate = _bankAccounts.Any(b =>
                !b.IsDeleted &&
                b.BankCode == bankCode.ToUpperInvariant() &&
                b.AccountNumber == accountNumber);

            Guard.Against(isDuplicate, "Bank account already exists");

            if (!_bankAccounts.Any(b => !b.IsDeleted) || isDefault)
            {
                foreach (var acc in _bankAccounts.Where(b => !b.IsDeleted))
                {
                    acc.RemoveDefault();
                }
                isDefault = true;
            }

            var bankAccount = BankAccount.Create(
                Id,
                bankCode,
                bankName,
                accountNumber,
                accountHolder,
                isDefault);

            _bankAccounts.Add(bankAccount);
            UpdatedAt = DateTime.UtcNow;

            //AddDomainEvent(new BankAccountAddedEvent(Id, bankAccount.Id));
        }

        public void RemoveBankAccount(Guid bankAccountId)
        {
            var account = _bankAccounts.FirstOrDefault(b => b.Id == bankAccountId && !b.IsDeleted);
            Guard.Against(account == null, "Bank account not found");

            var wasDefault = account!.IsDefault;
            account.SoftDelete();

            if (wasDefault)
            {
                var nextDefault = _bankAccounts.FirstOrDefault(b => !b.IsDeleted);
                nextDefault?.SetAsDefault();
            }

            UpdatedAt = DateTime.UtcNow;

            //AddDomainEvent(new BankAccountRemovedEvent(Id, bankAccountId));
        }

        public void SetDefaultBankAccount(Guid bankAccountId)
        {
            var account = _bankAccounts.FirstOrDefault(b => b.Id == bankAccountId && !b.IsDeleted);
            Guard.Against(account == null, "Bank account not found");

            foreach (var acc in _bankAccounts.Where(b => !b.IsDeleted))
            {
                acc.RemoveDefault();
            }

            account!.SetAsDefault();
            UpdatedAt = DateTime.UtcNow;
        }

        public void VerifyBankAccount(Guid bankAccountId)
        {
            var account = _bankAccounts.FirstOrDefault(b => b.Id == bankAccountId && !b.IsDeleted);
            Guard.Against(account == null, "Bank account not found");

            account!.Verify();
            UpdatedAt = DateTime.UtcNow;

            //AddDomainEvent(new BankAccountVerifiedEvent(Id, bankAccountId));
        }

        public BankAccount? GetDefaultBankAccount()
        {
            return _bankAccounts.FirstOrDefault(b => b.IsDefault && !b.IsDeleted);
        }
    }
}
