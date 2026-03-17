namespace Daco.Application.Users.Commands.AccountStatus
{
    public class SuspendUserCommandValidator : BaseValidator<SuspendUserCommand>
    {
        public SuspendUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
