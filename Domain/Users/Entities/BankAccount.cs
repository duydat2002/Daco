using Domain.Common;
using Shared.Common;

namespace Domain.Users.Entities
{
    public class BankAccount : BaseEntity
    {
        public Guid UserId { get; private set; }

        public string BankCode { get; private set; }       // VCB, TCB, ACB
        public string BankName { get; private set; }       // Vietcombank, Techcombank
        public string AccountNumber { get; private set; }  // Số tài khoản
        public string AccountHolder { get; private set; }  // Chủ tài khoản

        // Status
        public bool IsDefault { get; private set; }
        public bool IsVerified { get; private set; }
        public DateTime? VerifiedAt { get; private set; }

        // Timestamps
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        private BankAccount() { }

        public static BankAccount Create(
            Guid userId,
            string bankCode,
            string bankName,
            string accountNumber,
            string accountHolder,
            bool isDefault = false
        )
        {
            Guard.AgainstNullOrEmpty(bankCode, nameof(bankCode));
            Guard.AgainstNullOrEmpty(bankName, nameof(bankName));
            Guard.AgainstNullOrEmpty(accountNumber, nameof(accountNumber));
            Guard.AgainstNullOrEmpty(accountHolder, nameof(accountHolder));

            return new BankAccount
            {
                UserId = userId,
                BankCode = bankCode.ToUpperInvariant(),
                BankName = bankName,
                AccountNumber = accountNumber,
                AccountHolder = accountHolder,
                IsDefault = isDefault,
                IsVerified = false,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void Verify()
        {
            Guard.Against(IsVerified, "Bank account is already verified");

            IsVerified = true;
            VerifiedAt = DateTime.UtcNow;
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

        public bool IsDeleted => DeletedAt.HasValue;
    }
}
