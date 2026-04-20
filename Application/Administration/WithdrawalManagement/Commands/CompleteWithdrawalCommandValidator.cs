namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public class CompleteWithdrawalCommandValidator : BaseValidator<CompleteWithdrawalCommand>
    {
        public CompleteWithdrawalCommandValidator() 
        {
            RuleFor(x => x.WithdrawalId)
                .NotEmpty().WithMessage("WithdrawalId is required");

            RuleFor(x => x.TransactionCode)
                .NotEmpty().WithMessage("Transaction code is required")
                .MaximumLength(100).WithMessage("Transaction code must not exceed 100 characters");

            RuleFor(x => x.AdminNote)
                .MaximumLength(500)
                .When(x => x.AdminNote is not null);
        }
    }
}
