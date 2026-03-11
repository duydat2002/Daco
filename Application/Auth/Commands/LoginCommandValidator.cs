using System.ComponentModel.DataAnnotations;

namespace Daco.Application.Auth.Commands
{
    public class LoginCommandValidator : BaseValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Email or phone is required")
                .Must(x => IsEmail(x) || IsPhone(x))
                .WithMessage("Identifier must be a valid email or phone number");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }

        private static bool IsEmail(string value)
            => !string.IsNullOrEmpty(value) && new EmailAddressAttribute().IsValid(value);

        private static bool IsPhone(string value)
            => !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^(\+84|0)[0-9]{9}$");
    }
}
