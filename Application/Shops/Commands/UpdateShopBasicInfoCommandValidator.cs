namespace Daco.Application.Shops.Commands
{
    public class UpdateShopBasicInfoCommandValidator : BaseValidator<UpdateShopBasicInfoCommand>
    {
        public UpdateShopBasicInfoCommandValidator()
        {
            RuleFor(x => x.ShopName)
                .MinimumLength(3).WithMessage("Shop name must be at least 3 characters")
                .MaximumLength(255).WithMessage("Shop name must not exceed 255 characters")
                .When(x => x.ShopName is not null);

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters")
                .When(x => x.Description is not null);

            RuleFor(x => x.ShopEmail)
                .EmailAddress().WithMessage("Invalid email format")
                .When(x => x.ShopEmail is not null);

            RuleFor(x => x.ShopPhone)
                .ValidPhoneNumber()
                .When(x => x.ShopPhone is not null);

            RuleFor(x => x)
                .Must(x => x.ShopName is not null
                        || x.Description is not null
                        || x.ShopEmail is not null
                        || x.ShopPhone is not null)
                .WithMessage("At least one field must be provided")
                .OverridePropertyName("Request");
        }
    }
}
