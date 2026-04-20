namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public class ApproveWithdrawalCommandValidator : BaseValidator<ApproveWithdrawalCommand>
    {
        public ApproveWithdrawalCommandValidator()
        {
            RuleFor(x => x.WithdrawalId)
                .NotEmpty().WithMessage("WithdrawalId is required");

            RuleFor(x => x.AdminNote)
                .MaximumLength(500)
                .When(x => x.AdminNote is not null);
        }
    }
}
