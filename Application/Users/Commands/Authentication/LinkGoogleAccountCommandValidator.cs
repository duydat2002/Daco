namespace Daco.Application.Users.Commands.Authentication
{
    public class LinkGoogleAccountCommandValidator : BaseValidator<LinkGoogleAccountCommand>
    {
        public LinkGoogleAccountCommandValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty().WithMessage("GoogleId is required");
        }
    }
}
