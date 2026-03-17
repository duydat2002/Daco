namespace Daco.Application.Users.Commands.Profile
{
    public class UpdatePhoneCommandValidator : BaseValidator<UpdatePhoneCommand>
    {
        public UpdatePhoneCommandValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .ValidPhoneNumber();
        }
    }
}
