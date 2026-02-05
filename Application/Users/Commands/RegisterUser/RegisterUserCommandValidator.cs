namespace Daco.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : BaseValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username).ValidUsername();

            RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.Email) || !string.IsNullOrEmpty(x.Phone))
                .WithMessage("Either email or phone must be provided");

            RuleForNotEmpty(x => x.Email, r => r.EmailAddress().WithMessage("Invalid email format"));

            RuleForNotEmpty(x => x.Phone, r => r.ValidPhoneNumber());
        }
    }
}
