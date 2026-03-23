namespace Daco.Application.Administration.AdminManagement
{
    public class UpdateAdminStatusCommandValidator : BaseValidator<UpdateAdminStatusCommand>
    {
        private static readonly string[] AllowedStatuses = ["active", "inactive", "suspended"];

        public UpdateAdminStatusCommandValidator()
        {
            RuleFor(x => x.AdminId)
                .NotEmpty().WithMessage("AdminId is required");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .Must(s => AllowedStatuses.Contains(s))
                .WithMessage($"Status must be one of: {string.Join(", ", AllowedStatuses)}");

            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required when suspending")
                .When(x => x.Status == "suspended");
        }
    }
}
