namespace Daco.Application.Shops.Commands
{
    public class AddShopAddressCommandValidator : BaseValidator<AddShopAddressCommand>
    {
        public AddShopAddressCommandValidator() 
        {
            RuleFor(x => x.Label)
                .MaximumLength(100)
                .When(x => x.Label is not null);

            RuleFor(x => x.AddressType)
                .IsInEnum();

            RuleFor(x => x.ContactName)
                .MaximumLength(255)
                .NotEmpty();

            RuleFor(x => x.ContactPhone)
                .MaximumLength(20)
                .ValidPhoneNumber()
                .NotEmpty();

            RuleFor(x => x.City)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(x => x.District)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(x => x.Ward)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(x => x.AddressDetail)
                .NotEmpty();

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .NotEmpty();

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .NotEmpty();
        }
    }
}
