namespace Daco.Application.Auth.Commands
{
    public class RefreshTokenCommandValidator : BaseValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required");
        }
    }
}
