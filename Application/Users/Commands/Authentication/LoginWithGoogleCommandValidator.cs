namespace Daco.Application.Users.Commands.Authentication
{
    public class LoginWithGoogleCommandValidator : BaseValidator<LoginWithGoogleCommand>
    {
        public LoginWithGoogleCommandValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty()
                .WithMessage("Google token is required");
        }
    }
}
