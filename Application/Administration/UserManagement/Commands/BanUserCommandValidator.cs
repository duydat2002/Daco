namespace Daco.Application.Administration.UserManagement.Commands
{
    public class BanUserCommandValidator : BaseValidator<BanUserCommand>
    {
        public BanUserCommandValidator() 
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required")
                .MaximumLength(500);
        }
    }
}
