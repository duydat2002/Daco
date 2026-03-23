namespace Daco.Application.Administration.AdminManagement
{
    public class RevokeAdminRoleCommandValidator : BaseValidator<RevokeAdminRoleCommand>
    {
        public RevokeAdminRoleCommandValidator()
        {
            RuleFor(x => x.AdminId)
                    .NotEmpty().WithMessage("AdminId is required");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("RoleId is required");
        }
    }
}
