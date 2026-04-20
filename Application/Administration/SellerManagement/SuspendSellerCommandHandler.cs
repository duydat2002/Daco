namespace Daco.Application.Administration.SellerManagement
{
    public class SuspendSellerCommandHandler : IRequestHandler<SuspendSellerCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SuspendSellerCommandHandler> _logger;

        public SuspendSellerCommandHandler(
            IAdminUserRepository adminRepository,
            ISellerRepository sellerRepository,
            IUnitOfWork unitOfWork,
            ILogger<SuspendSellerCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _sellerRepository = sellerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SuspendSellerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin suspending seller {SellerId}", request.SellerId);

            var seller = await _sellerRepository.GetByIdAsync(request.SellerId, cancellationToken);
            if (seller is null)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.NotFound, "Seller not found");

            if (seller.Status == SellerStatus.Suspended)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.AlreadySuppended, "Seller is already suppended");

            seller.Suspend(request.SuspendedBy, request.Reason);
            _unitOfWork.TrackEntity(seller);

            _logger.LogInformation("Seller {SellerId} suspending by admin", request.SellerId);

            return ResponseDTO.Success(new
            {
                request.SellerId,
                Status = SellerStatus.Suspended.ToString().ToLower(),
                request.Reason
            }, "Seller suspended successfully");
        }
    }
}
