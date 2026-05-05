namespace Daco.Application.Shops.Commands.Addresses
{
    public class SetDefaultShopAddressCommandValidator : BaseValidator<SetDefaultShopAddressCommand>
    {
        public SetDefaultShopAddressCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.AddressId)
                .NotEmpty();
        }
    }
}
