namespace Daco.Application.Users.Commands.RegisterUser
{
    public class RegisterWithGoogleCommandValidator : BaseValidator<RegisterWithGoogleCommand>
    {
        public RegisterWithGoogleCommandValidator()
        {
            RuleFor(x => x.IdToken)
                .NotEmpty()
                .WithMessage("Google token is required");
        }
    }
}
