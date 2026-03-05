namespace Daco.Application.Users.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : BaseValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Username).ValidUsername();

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .ValidPhoneNumber();

            RuleForNotEmpty(x => x.Password, r => r.StrongPassword());
        }
    }
}
