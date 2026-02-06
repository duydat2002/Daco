namespace Daco.Domain.Users.Aggregates
{
    public class User : AggregateRoot
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
        public UserGender Gender { get; private set; }
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

        internal static User Reconstitute(
            Guid id,
            string username,
            string? email,
            string? phone,
            string? name,
            string? avatar,
            DateTime? dateOfBirth,
            UserGender gender,
            UserStatus status,
            bool emailVerified,
            bool phoneVerified,
            DateTime createdAt,
            DateTime? updatedAt,
            DateTime? deletedAt
        )
        {
            var user = new User
            {
                Username = Username.Create(username),
                Email = !string.IsNullOrEmpty(email) ? Email.Create(email) : null,
                Phone = !string.IsNullOrEmpty(phone) ? PhoneNumber.Create(phone) : null,
                Name = name,
                Avatar = avatar,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Status = status,
                EmailVerified = emailVerified,
                PhoneVerified = phoneVerified,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                DeletedAt = deletedAt
            };

            user.SetId(id);

            return user;
        }

        #region user
        #region auth
        public static User CreateWithEmail(string username, string email, string passwordHash)
        {
            var usernameVo = Username.Create(username);
            var emailVo = Email.Create(email);

            var user = new User
            {
                Username = usernameVo,
                Email = emailVo,
                Status = UserStatus.Active,
                EmailVerified = false,
                PhoneVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            var emailProvider = AuthProvider.CreateEmailProvider(emailVo.Value, passwordHash);
            user._authProviders.Add(emailProvider);

            user.AddDomainEvent(new UserRegisteredEvent(
                user.Id,
                emailVo.Value,
                ProviderType.Email));

            return user;
        }

        public static User CreateWithPhone(string username, string phone, string passwordHash)
        {
            var usernameVo = Username.Create(username);
            var phoneVo = PhoneNumber.Create(phone);

            var user = new User
            {
                Username = usernameVo,
                Phone = phoneVo,
                Status = UserStatus.Active,
                EmailVerified = false,
                PhoneVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            var phoneProvider = AuthProvider.CreatePhoneProvider(phoneVo.Value, passwordHash);
            user._authProviders.Add(phoneProvider);

            user.AddDomainEvent(new UserRegisteredEvent(
                user.Id,
                phoneVo.Value,
                ProviderType.Phone));

            return user;
        }

        public static User CreateWithSocial(
            string username,
            ProviderType providerType,
            string providerUserId,
            string? email,
            string? name,
            string? avatar)
        {
            var usernameVo = Username.Create(username);

            var user = new User
            {
                Username = usernameVo,
                Email = !string.IsNullOrEmpty(email) ? Email.Create(email) : null,
                Name = name,
                Status = UserStatus.Active,
                EmailVerified = !string.IsNullOrEmpty(email),
                PhoneVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            var socialProvider = AuthProvider.CreateSocialProvider(
                providerType,
                providerUserId,
                email,
                name,
                avatar);

            user._authProviders.Add(socialProvider);

            user.AddDomainEvent(new UserRegisteredEvent(
                user.Id,
                email ?? providerUserId,
                providerType));

            return user;
        }
        #endregion

        #region verification
        public void VerifyEmail()
        {
            Guard.Against(Email == null, "User does not have an email");
            Guard.Against(EmailVerified, "Email is already verified");
            Guard.Against(Status != UserStatus.Active, "Cannot verify email for inactive user");

            EmailVerified = true;
            UpdatedAt = DateTime.UtcNow;

            var emailProvider = _authProviders
                .FirstOrDefault(p => p.ProviderType == ProviderType.Email);

            emailProvider?.MarkAsVerified();

            AddDomainEvent(new EmailVerifiedEvent(Id, Email!.Value));
        }

        public void VerifyPhone()
        {
            Guard.Against(Phone == null, "User does not have a phone");
            Guard.Against(PhoneVerified, "Phone is already verified");
            Guard.Against(Status != UserStatus.Active, "Cannot verify phone for inactive user");

            PhoneVerified = true;
            UpdatedAt = DateTime.UtcNow;

            var phoneProvider = _authProviders
                .FirstOrDefault(p => p.ProviderType == ProviderType.Phone);

            phoneProvider?.MarkAsVerified();

            AddDomainEvent(new PhoneVerifiedEvent(Id, Phone!.Value));
        }
        #endregion

        #region profile management
        public void UpdateProfile(string? name, DateTime? dateOfBirth, UserGender gender)
        {
            Guard.Against(Status != UserStatus.Active, "Cannot update profile for inactive user");

            if (dateOfBirth.HasValue)
            {
                var age = DateTime.UtcNow.Year - dateOfBirth.Value.Year;
                Guard.Against(age < 13, "User must be at least 13 years old");
                Guard.Against(age > 150, "Invalid date of birth");
            }

            Name = name;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateAvatar(string avatarUrl)
        {
            Guard.AgainstNullOrEmpty(avatarUrl, nameof(avatarUrl));
            Guard.Against(Status != UserStatus.Active, "Cannot update avatar for inactive user");

            Avatar = avatarUrl;
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        #region password management
        public void ChangePassword(string currentPasswordHash, string newPasswordHash)
        {
            Guard.Against(Status != UserStatus.Active, "Cannot change password for inactive user");

            var provider = _authProviders
                .FirstOrDefault(p => p.ProviderType == ProviderType.Email || p.ProviderType == ProviderType.Phone);

            Guard.Against(provider == null, "User does not have email/phone provider");
            Guard.Against(provider!.PasswordHash != currentPasswordHash, "Current password is incorrect");

            provider.UpdatePassword(newPasswordHash);
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new PasswordChangedEvent(Id));
        }
        #endregion

        #region address management
        public void AddAddress(UserAddress address)
        {
            Guard.AgainstNull(address, nameof(address));
            Guard.Against(Status != UserStatus.Active, "Cannot add address for inactive user");
            Guard.Against(_addresses.Count(a => !a.IsDeleted) >= 10,
                "Cannot have more than 10 active addresses");

            if (!_addresses.Any(a => !a.IsDeleted) || address.IsDefault)
            {
                foreach (var addr in _addresses.Where(a => !a.IsDeleted))
                {
                    addr.RemoveDefault();
                }
            }

            _addresses.Add(address);
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new AddressAddedEvent(Id, address.Id));
        }

        public void UpdateAddress(Guid addressId, UserAddress updatedAddress)
        {
            Guard.Against(Status != UserStatus.Active, "Cannot update address for inactive user");

            var address = _addresses.FirstOrDefault(a => a.Id == addressId && !a.IsDeleted);
            Guard.Against(address == null, "Address not found");

            address!.Update(
                updatedAddress.RecipientName,
                updatedAddress.RecipientPhone,
                updatedAddress.City,
                updatedAddress.District,
                updatedAddress.Ward,
                updatedAddress.AddressDetail,
                updatedAddress.Label);

            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveAddress(Guid addressId)
        {
            var address = _addresses.FirstOrDefault(a => a.Id == addressId && !a.IsDeleted);
            Guard.Against(address == null, "Address not found");

            var wasDefault = address!.IsDefault;
            address.SoftDelete();

            if (wasDefault)
            {
                var nextDefault = _addresses.FirstOrDefault(a => !a.IsDeleted);
                nextDefault?.SetAsDefault();
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDefaultAddress(Guid addressId)
        {
            var address = _addresses.FirstOrDefault(a => a.Id == addressId && !a.IsDeleted);
            Guard.Against(address == null, "Address not found");

            foreach (var addr in _addresses.Where(a => !a.IsDeleted))
            {
                addr.RemoveDefault();
            }

            address!.SetAsDefault();
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        #region status management
        public void Suspend(string reason)
        {
            Guard.Against(Status == UserStatus.Banned, "Cannot suspend a banned user");
            Guard.Against(Status == UserStatus.Deleted, "Cannot suspend a deleted user");

            Status = UserStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new UserSuspendedEvent(Id, reason));
        }

        public void Ban(string reason)
        {
            Guard.Against(Status == UserStatus.Deleted, "Cannot ban a deleted user");

            Status = UserStatus.Banned;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Activate()
        {
            Guard.Against(Status == UserStatus.Deleted, "Cannot activate a deleted user");

            Status = UserStatus.Active;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Delete()
        {
            Status = UserStatus.Deleted;
            DeletedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        #endregion

        #region bacnk account
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
        #endregion

        public bool HasVerifiedContact()
        {
            return EmailVerified || PhoneVerified;
        }

        public bool CanLogin()
        {
            return Status == UserStatus.Active && HasVerifiedContact();
        }

        public UserAddress? GetDefaultAddress()
        {
            return _addresses.FirstOrDefault(a => a.IsDefault && !a.IsDeleted);
        }

        public IEnumerable<UserAddress> GetActiveAddresses()
        {
            return _addresses.Where(a => !a.IsDeleted);
        }
    }
}
