namespace Daco.Application.Auth.Commands
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
