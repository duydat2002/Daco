namespace Daco.Application.Administration.ProductManagement.Commands
{
    public class ApproveProductCommandValidator : BaseValidator<ApproveProductCommand>
    {
        public ApproveProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}