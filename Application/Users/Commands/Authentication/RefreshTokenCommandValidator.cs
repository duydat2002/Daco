namespace Daco.Application.Users.Commands.Authentication
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
