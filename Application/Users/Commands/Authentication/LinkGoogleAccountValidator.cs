namespace Daco.Application.Users.Commands.Authentication
{
    public class LinkGoogleAccountValidator : BaseValidator<LinkGoogleAccountCommand>
    {
        public LinkGoogleAccountValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("GoogleId is required");
        }
    }
}
