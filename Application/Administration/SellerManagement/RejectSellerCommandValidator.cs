namespace Daco.Application.Administration.SellerManagement
{
    public class RejectSellerCommandValidator : BaseValidator<RejectSellerCommand>
    {
        public RejectSellerCommandValidator()
        {
            RuleFor(x => x.SellerId).NotEmpty();
            RuleFor(x => x.Reason).NotEmpty();
        }
    }
}
