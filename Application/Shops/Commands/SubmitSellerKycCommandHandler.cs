namespace Daco.Application.Shops.Commands
{
    public class SubmitSellerKycCommandHandler : IRequestHandler<SubmitSellerKycCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SubmitSellerKycCommand> _logger;

        public SubmitSellerKycCommandHandler(
            ISellerRepository sellerRepository,
            IUnitOfWork unitOfWork,
            ILogger<SubmitSellerKycCommand> logger)
        {
            _sellerRepository = sellerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SubmitSellerKycCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("KYC submission for user {UserId}", request.UserId);

            var seller = await _sellerRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (seller is null)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound,
                    "Seller record not found. Please onboard first.");

            if (seller.Status == SellerStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.AlreadyExists,
                    "Seller is already verified and active");

            if (seller.Status == SellerStatus.Banned)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.AccountBanned,
                    "This seller account has been banned");

            if (seller.Status == SellerStatus.Suspended)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.AccountSuspended,
                    "This seller account is suspended");

            if (request.BusinessType == BusinessTypes.Individual)
                seller.SubmitIndividualKyc(
                    request.IdentityNumber!,
                    request.IdentityFrontUrl!,
                    request.IdentityBackUrl!);
            else
                seller.SubmitCompanyKyc(
                    request.CompanyName!,
                    request.BusinessLicenseNumber!,
                    request.BusinessLicenseUrl!,
                    request.TaxCode!);

            await _sellerRepository.UpdateAsync(seller, cancellationToken);
            _unitOfWork.TrackEntity(seller);

            _logger.LogInformation("KYC submitted for seller {SellerId}", seller.Id);

            return ResponseDTO.Success(new
            {
                seller.Id,
                seller.BusinessType,
                Status = seller.Status.ToString().ToLower()
            }, "KYC submitted successfully. Awaiting admin review.");
        }
    }
}
