namespace Daco.Domain.Shops.Aggregates
{
    public class Seller : AggregateRoot
    {
        // Identity
        public Guid UserId { get; private set; }
        public string BusinessType { get; private set; }  // "individual" | "company"
        public SellerStatus Status { get; private set; } //'pending', 'active', 'suspended', 'banned'
        public bool IsVerified { get; private set; }
        public bool IsOfficial { get; private set; }
        public DateTime? VerifiedAt { get; private set; }

        // Individual KYC
        public string? IdentityNumber { get; private set; }
        public string? IdentityFrontUrl { get; private set; }
        public string? IdentityBackUrl { get; private set; }
        public bool IdentityVerified { get; private set; }

        // Company KYC
        public string? CompanyName { get; private set; }
        public string? BusinessLicenseNumber { get; private set; }
        public string? BusinessLicenseUrl { get; private set; }
        public string? TaxCode { get; private set; }

        // Timestamps
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        protected Seller() { }

        #region Register
        public static Seller RegisterIndividual(
            Guid userId,
            string identityNumber,
            string identityFrontUrl,
            string identityBackUrl)
        {
            Guard.Against(userId == Guid.Empty, "UserId is required");
            Guard.AgainstNullOrEmpty(identityNumber, nameof(identityNumber));
            Guard.AgainstNullOrEmpty(identityFrontUrl, nameof(identityFrontUrl));
            Guard.AgainstNullOrEmpty(identityBackUrl, nameof(identityBackUrl));

            var seller = new Seller
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BusinessType = BusinessTypes.Individual,
                Status = SellerStatus.Pending,
                IsVerified = false,
                IsOfficial = false,
                IdentityNumber = identityNumber,
                IdentityFrontUrl = identityFrontUrl,
                IdentityBackUrl = identityBackUrl,
                IdentityVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            seller.AddDomainEvent(new SellerRegisteredEvent(seller.Id, userId, BusinessTypes.Individual));
            return seller;
        }

        public static Seller RegisterCompany(
            Guid userId,
            string companyName,
            string businessLicenseNumber,
            string businessLicenseUrl,
            string taxCode)
        {
            Guard.Against(userId == Guid.Empty, "UserId is required");
            Guard.AgainstNullOrEmpty(companyName, nameof(companyName));
            Guard.AgainstNullOrEmpty(businessLicenseNumber, nameof(businessLicenseNumber));
            Guard.AgainstNullOrEmpty(businessLicenseUrl, nameof(businessLicenseUrl));
            Guard.AgainstNullOrEmpty(taxCode, nameof(taxCode));

            var seller = new Seller
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BusinessType = BusinessTypes.Company,
                Status = SellerStatus.Pending,
                IsVerified = false,
                IsOfficial = false,
                CompanyName = companyName,
                BusinessLicenseNumber = businessLicenseNumber,
                BusinessLicenseUrl = businessLicenseUrl,
                TaxCode = taxCode,
                CreatedAt = DateTime.UtcNow
            };

            seller.AddDomainEvent(new SellerRegisteredEvent(seller.Id, userId, BusinessTypes.Company));
            return seller;
        }

        internal static Seller Reconstitute(
            Guid id,
            Guid userId,
            string businessType,
            SellerStatus status,
            bool isVerified,
            bool isOfficial,
            DateTime? verifiedAt,
            string? identityNumber,
            string? identityFrontUrl,
            string? identityBackUrl,
            bool identityVerified,
            string? companyName,
            string? businessLicenseNumber,
            string? businessLicenseUrl,
            string? taxCode,
            DateTime createdAt,
            DateTime? updatedAt,
            DateTime? deletedAt)
        {
            return new Seller
            {
                Id = id,
                UserId = userId,
                BusinessType = businessType,
                Status = status,
                IsVerified = isVerified,
                IsOfficial = isOfficial,
                VerifiedAt = verifiedAt,
                IdentityNumber = identityNumber,
                IdentityFrontUrl = identityFrontUrl,
                IdentityBackUrl = identityBackUrl,
                IdentityVerified = identityVerified,
                CompanyName = companyName,
                BusinessLicenseNumber = businessLicenseNumber,
                BusinessLicenseUrl = businessLicenseUrl,
                TaxCode = taxCode,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt,
                DeletedAt = deletedAt
            };
        }
        #endregion

        #region Verification 
        public void Approve(Guid approvedByAdminId)
        {
            Guard.Against(Status != SellerStatus.Pending, "Only pending sellers can be approved");

            Status = SellerStatus.Active;
            IsVerified = true;
            VerifiedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new SellerApprovedEvent(Id, UserId, approvedByAdminId));
        }

        public void Reject(string reason)
        {
            Guard.Against(Status != SellerStatus.Pending, "Only pending sellers can be rejected");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            Status = SellerStatus.Banned;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new SellerRejectedEvent(Id, UserId, reason));
        }

        public void Suspend(string reason)
        {
            Guard.Against(Status != SellerStatus.Active, "Only active sellers can be suspended");
            Guard.AgainstNullOrEmpty(reason, nameof(reason));

            Status = SellerStatus.Suspended;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new SellerSuspendedEvent(Id, UserId, reason));
        }

        public void Reinstate()
        {
            Guard.Against(Status != SellerStatus.Suspended, "Only suspended sellers can be reinstated");

            Status = SellerStatus.Active;
            UpdatedAt = DateTime.UtcNow;

            AddDomainEvent(new SellerReinstatedEvent(Id, UserId));
        }

        public void MarkAsOfficial()
        {
            Guard.Against(!IsVerified, "Seller must be verified before marking as official");
            Guard.Against(Status != SellerStatus.Active, "Seller must be active");

            IsOfficial = true;
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        #region KYC update
        public void ResubmitIdentity(
            string identityNumber,
            string identityFrontUrl,
            string identityBackUrl)
        {
            Guard.Against(BusinessType != BusinessTypes.Individual, "Only individual sellers can resubmit identity");
            Guard.Against(Status == SellerStatus.Active, "Cannot resubmit when already active");

            IdentityNumber = identityNumber;
            IdentityFrontUrl = identityFrontUrl;
            IdentityBackUrl = identityBackUrl;
            IdentityVerified = false;
            Status = SellerStatus.Pending;
            UpdatedAt = DateTime.UtcNow;
        }
        #endregion

        public bool IsActive => Status == SellerStatus.Active;
        public bool IsIndividual => BusinessType == BusinessTypes.Individual;
        public bool IsCompany => BusinessType == BusinessTypes.Company;
    }
}
