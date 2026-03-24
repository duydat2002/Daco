namespace Daco.Application.Administration.UserManagement.Commands
{
    public class ActivateUserCommandValidator : BaseValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator() 
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
