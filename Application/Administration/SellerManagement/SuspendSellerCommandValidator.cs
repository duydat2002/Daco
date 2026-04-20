namespace Daco.Application.Administration.SellerManagement
{
    public class SuspendSellerCommandValidator : BaseValidator<SuspendSellerCommand>
    {
        public SuspendSellerCommandValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}
