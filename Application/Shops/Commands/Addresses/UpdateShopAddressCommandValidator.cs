using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daco.Application.Shops.Commands.Addresses
{
    public class UpdateShopAddressCommandValidator : BaseValidator<UpdateShopAddressCommand>
    {
        public UpdateShopAddressCommandValidator()
        {
            RuleFor(x => x.AddressId)
                .NotEmpty();

            RuleFor(x => x.AddressType)
                .IsInEnum();

            RuleFor(x => x.ContactName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.ContactPhone)
                .NotEmpty()
                .MaximumLength(20)
                .ValidPhoneNumber();

            RuleFor(x => x.City)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.District)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Ward)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.AddressDetail)
                .NotEmpty();

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180);

            RuleFor(x => x.Label)
                .MaximumLength(100)
                .When(x => x.Label is not null);
        }
    }
}
