namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public class RejectWithdrawalCommandValidator : BaseValidator<RejectWithdrawalCommand>
    {
        public RejectWithdrawalCommandValidator() 
        {
            RuleFor(x => x.WithdrawalId)
                .NotEmpty().WithMessage("WithdrawalId is required");

            RuleFor(x => x.RejectedReason)
                .NotEmpty().WithMessage("Rejection reason is required")
                .MaximumLength(500).WithMessage("Reason must not exceed 500 characters");

            RuleFor(x => x.AdminNote)
                .MaximumLength(500)
                .When(x => x.AdminNote is not null);
        }
    }
}
