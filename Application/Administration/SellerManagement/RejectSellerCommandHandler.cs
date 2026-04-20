namespace Daco.Application.Administration.SellerManagement
{
    public class RejectSellerCommandHandler : IRequestHandler<RejectSellerCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RejectSellerCommandHandler> _logger;

        public RejectSellerCommandHandler(
            IAdminUserRepository adminRepository,
            ISellerRepository sellerRepository,
            IUnitOfWork unitOfWork,
            ILogger<RejectSellerCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _sellerRepository = sellerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(RejectSellerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin rejecting seller {SellerId}", request.SellerId);

            var seller = await _sellerRepository.GetByIdAsync(request.SellerId, cancellationToken);
            if (seller is null)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.NotFound, "Seller not found");

            if (seller.Status == SellerStatus.Banned)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.AlreadyBanned, "Seller is already banned");

            seller.Reject(request.RejectedBy, request.Reason);
            _unitOfWork.TrackEntity(seller);

            _logger.LogInformation("Seller {SellerId} rejecting by admin", request.SellerId);

            return ResponseDTO.Success(new
            {
                request.SellerId,
                Status = SellerStatus.Banned.ToString().ToLower(),
                request.Reason
            }, "Seller rejected successfully");
        }
    }
}
