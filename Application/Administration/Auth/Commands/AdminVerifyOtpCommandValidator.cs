namespace Daco.Application.Administration.Auth.Commands
{
    public class AdminVerifyOtpCommandValidator : BaseValidator<AdminVerifyOtpCommand>
    {
        public AdminVerifyOtpCommandValidator()
        {
            RuleFor(x => x.TempToken)
                .NotEmpty().WithMessage("TempToken is required");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP is required")
                .Length(6).WithMessage("OTP must be 6 digits")
                .Matches(@"^\d{6}$").WithMessage("OTP must contain only digits");
        }
    }
}
