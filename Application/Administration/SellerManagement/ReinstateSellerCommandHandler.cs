namespace Daco.Application.Administration.SellerManagement
{
    public class ReinstateSellerCommandHandler : IRequestHandler<ReinstateSellerCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReinstateSellerCommandHandler> _logger;

        public ReinstateSellerCommandHandler(
            IAdminUserRepository adminRepository,
            ISellerRepository sellerRepository,
            IUnitOfWork unitOfWork,
            ILogger<ReinstateSellerCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _sellerRepository = sellerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(ReinstateSellerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin reinstating seller {SellerId}", request.SellerId);

            var seller = await _sellerRepository.GetByIdAsync(request.SellerId, cancellationToken);
            if (seller is null)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.NotFound, "Seller not found");

            if (seller.Status == SellerStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.AlreadyActivated, "Seller is already actived");

            seller.Reinstate(request.ReinstatedBy);
            _unitOfWork.TrackEntity(seller);

            _logger.LogInformation("Seller {SellerId} reinstating by admin", request.SellerId);

            return ResponseDTO.Success(new
            {
                request.SellerId,
                Status = SellerStatus.Active.ToString().ToLower()
            }, "Seller reinstated successfully");
        }
    }
}
