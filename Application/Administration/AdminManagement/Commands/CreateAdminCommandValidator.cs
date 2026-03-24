namespace Daco.Application.Administration.AdminManagement.Commands
{
    public class CreateAdminCommandValidator : BaseValidator<CreateAdminCommand>
    {
        public CreateAdminCommandValidator()
        {
            RuleFor(x => x.Username)
                    .ValidUsername();

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .StrongPassword();

            RuleFor(x => x.EmployeeCode)
                .NotEmpty().WithMessage("Employee code is required")
                .MaximumLength(50).WithMessage("Employee code must not exceed 50 characters");

            RuleFor(x => x.RoleIds)
                .NotEmpty().WithMessage("At least one role is required");
        }
    }
}
