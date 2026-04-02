namespace Daco.Application.Shops.Commands
{
    public class SubmitSellerKycCommandValidator : BaseValidator<SubmitSellerKycCommand>
    {
        private static readonly string[] AllowedBusinessTypes = [BusinessTypes.Individual, BusinessTypes.Company];

        public SubmitSellerKycCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.BusinessType)
                .NotEmpty()
                .Must(p => AllowedBusinessTypes.Contains(p))
                .WithMessage($"Business type must be one of: {string.Join(", ", AllowedBusinessTypes)}"); ;

            When(x => x.BusinessType == BusinessTypes.Individual, () =>
            {
                RuleFor(x => x.IdentityNumber)
                    .NotEmpty().WithMessage("Identity number is required")
                    .Length(12).WithMessage("Identity number must be 12 digits")
                    .Matches(@"^\d{12}$").WithMessage("Identity number must be numeric");

                RuleFor(x => x.IdentityFrontUrl)
                    .NotEmpty().WithMessage("Identity front image is required")
                    .MustBeValidUrl();

                RuleFor(x => x.IdentityBackUrl)
                    .NotEmpty().WithMessage("Identity back image is required")
                    .MustBeValidUrl();
            });

            When(x => x.BusinessType == "company", () =>
            {
                RuleFor(x => x.CompanyName)
                    .NotEmpty().WithMessage("Company name is required");

                RuleFor(x => x.BusinessLicenseNumber)
                    .NotEmpty().WithMessage("Business license number is required");

                RuleFor(x => x.BusinessLicenseUrl)
                    .NotEmpty().WithMessage("Business license file is required")
                    .MustBeValidUrl();

                RuleFor(x => x.TaxCode)
                    .NotEmpty().WithMessage("Tax code is required")
                    .Matches(@"^\d{10}(\d{3})?$")
                    .WithMessage("Tax code must be 10 or 13 digits");
            });
        }
    }
}
