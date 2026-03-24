namespace Daco.Application.Administration.AdminManagement.Commands
{
    public class AssignAdminRoleCommandValidator : BaseValidator<AssignAdminRoleCommand>
    {
        public AssignAdminRoleCommandValidator()
        {
            RuleFor(x => x.AdminId)
                    .NotEmpty().WithMessage("AdminId is required");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required");

            RuleFor(x => x.ExpiresAt)
                .GreaterThan(DateTime.UtcNow).WithMessage("ExpiresAt must be in the future")
                .When(x => x.ExpiresAt.HasValue);
        }
    }
}
