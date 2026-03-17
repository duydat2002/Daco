namespace Daco.Application.Users.Commands.Profile
{
    public class UpdateUsernameCommandValidator : BaseValidator<UpdateUsernameCommand>
    {
        public UpdateUsernameCommandValidator()
        {
            RuleFor(x => x.Username).ValidUsername();
        }
    }
}
