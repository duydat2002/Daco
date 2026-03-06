namespace Daco.Application.Users.Commands.Verifications
{
    public class ResendOtpCommandValidator : BaseValidator<ResendOtpCommand>
    {
        public ResendOtpCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
