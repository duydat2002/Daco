namespace Daco.Application.Administration.SellerManagement
{
    public class ApproveSellerCommandValidator : BaseValidator<ApproveSellerCommand>
    {
        public ApproveSellerCommandValidator() 
        {
            RuleFor(x => x.SellerId).NotEmpty();
        }
    }
}
