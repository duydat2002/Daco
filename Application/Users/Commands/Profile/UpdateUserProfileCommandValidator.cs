namespace Daco.Application.Users.Commands.Profile
{
    public class UpdateUserProfileCommandValidator : BaseValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator() 
        {
            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(x => x.Name is not null);

            RuleFor(x => x.DateOfBirth)
                .ValidDateFormat()
                .Must(dob =>
                {
                    if (!DateTime.TryParseExact(dob, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                        return true; 
                    return date <= DateTime.UtcNow.AddYears(-13);
                })
                .WithMessage("User must be at least 13 years old")
                .When(x => x.DateOfBirth is not null && x.DateOfBirth != "");

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage("Invalid gender value");
        }
    }
}
