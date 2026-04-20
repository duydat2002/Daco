namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class SuspendProductCommandValidator : BaseValidator<SuspendProductCommand>
    {
        public SuspendProductCommandValidator() 
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}
