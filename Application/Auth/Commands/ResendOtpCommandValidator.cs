namespace Daco.Application.Auth.Commands
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
