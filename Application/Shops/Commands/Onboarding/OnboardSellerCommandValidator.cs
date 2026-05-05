namespace Daco.Application.Shops.Commands.Onboarding
{
    public class OnboardSellerCommandValidator : BaseValidator<OnboardSellerCommand>
    {

        public OnboardSellerCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
