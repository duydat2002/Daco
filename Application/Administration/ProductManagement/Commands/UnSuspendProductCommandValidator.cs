namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class UnSuspendProductCommandValidator : BaseValidator<SuspendProductCommand>
    {
        public UnSuspendProductCommandValidator() 
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}
