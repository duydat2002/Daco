namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class RemoveProductCommandValidator : BaseValidator<RemoveProductCommand>
    {
        public RemoveProductCommandValidator() 
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}
