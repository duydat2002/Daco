namespace Daco.Application.Administration.SellerManagement
{
    public class ReinstateSellerCommandValidator : BaseValidator<ReinstateSellerCommand>
    {
        public ReinstateSellerCommandValidator() 
        {
            RuleFor(x => x.SellerId).NotEmpty();
        }
    }
}
