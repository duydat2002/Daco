namespace Daco.Application.Administration.OrderManagement.Commands
{
    public class AdminCancelOrderCommandValidator : BaseValidator<AdminCancelOrderCommand>
    {
        public AdminCancelOrderCommandValidator() 
        {
            RuleFor(x => x.Reason)
                .NotEmpty().WithMessage("Reason is required")
                .MaximumLength(500).WithMessage("Reason must not exceed 500 characters");
        }
    }
}
