namespace Daco.Application.Auth.Commands
{
    public class VerifyEmailCommandValidator : BaseValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required")
                .Length(6).WithMessage("OTP must be 6 digits")
                .Matches(@"^\d{6}$").WithMessage("OTP must contain only digits");

            RuleFor(x => x.IpAddress)
                .NotEmpty().WithMessage("IP Address is required");
        }
    }
}
