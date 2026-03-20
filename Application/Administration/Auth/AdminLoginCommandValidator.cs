namespace Daco.Application.Administration.Auth
{
    public class AdminLoginCommandValidator : BaseValidator<AdminLoginCommand>
    {
        public AdminLoginCommandValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty().WithMessage("Email or username is required")
                .Must(x => IsEmail(x) || IsUsername(x))
                .WithMessage("Identifier must be a valid email or username");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }

        private static bool IsEmail(string value)
            => !string.IsNullOrEmpty(value) && new EmailAddressAttribute().IsValid(value);

        private static bool IsUsername(string value)
            => !string.IsNullOrEmpty(value) && Regex.IsMatch(value, @"^[a-zA-Z0-9_]+$") && value.Length >= 3 && value.Length <= 50;
    }
}
