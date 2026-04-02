namespace Daco.Application.Shops.Commands
{
    public class OnboardSellerCommandValidator : BaseValidator<OnboardSellerCommand>
    {

        public OnboardSellerCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
