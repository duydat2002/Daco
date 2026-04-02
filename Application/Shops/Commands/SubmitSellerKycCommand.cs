namespace Daco.Application.Shops.Commands
{
    public record SubmitSellerKycCommand : IRequest<ResponseDTO>
    {
        public Guid    UserId                { get; init; }
        public string  BusinessType          { get; init; } //individual, company
        //For Individual
        public string? IdentityNumber        { get; init; }
        public string? IdentityFrontUrl      { get; init; }
        public string? IdentityBackUrl       { get; init; }
        //For Company
        public string? CompanyName           { get; init; }
        public string? BusinessLicenseNumber { get; init; }
        public string? BusinessLicenseUrl    { get; init; }
        public string? TaxCode               { get; init; }
    }
}
