namespace Daco.Application.Users.Commands.Banking
{
    public class UpdateBankAccountCommandValidator : BaseValidator<UpdateBankAccountCommand>
    {
        public UpdateBankAccountCommandValidator() 
        {
            RuleFor(x => x.BankCode)
                    .NotEmpty()
                    .MaximumLength(20);

            RuleFor(x => x.BankName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.AccountNumber)
                .NotEmpty()
                .MaximumLength(50)
                .Matches(@"^\d+$").WithMessage("Account number must contain only digits");

            RuleFor(x => x.AccountHolder)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
