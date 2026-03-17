namespace Daco.Application.Users.Commands.Address
{
    public class AddAddressCommandValidator : BaseValidator<AddAddressCommand>
    {
        private static readonly string[] AllowedTypes = ["home", "office", "other"];

        public AddAddressCommandValidator() 
        {
            RuleFor(x => x.RecipientName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.RecipientPhone)
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

            RuleFor(x => x.Label)
                .MaximumLength(100)
                .When(x => x.Label is not null);

            RuleFor(x => x.AddressType)
                .Must(t => AllowedTypes.Contains(t))
                .WithMessage("Address type must be home, office, or other");
        }
    }
}
