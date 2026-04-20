namespace Daco.Application.Administration.SellerManagement
{
    public class ApproveSellerCommandHandler : IRequestHandler<ApproveSellerCommand, ResponseDTO>
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApproveSellerCommandHandler> _logger;

        public ApproveSellerCommandHandler(
            IAdminUserRepository adminRepository, 
            ISellerRepository sellerRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<ApproveSellerCommandHandler> logger)
        {
            _adminRepository = adminRepository;
            _sellerRepository = sellerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(ApproveSellerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin approving seller {SellerId}", request.SellerId);

            var seller = await _sellerRepository.GetByIdAsync(request.SellerId, cancellationToken);
            if (seller is null)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.NotFound, "Seller not found");

            if (seller.Status == SellerStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.AlreadyActivated, "Seller is already active");

            seller.Approve(request.ApprovedBy);
            _unitOfWork.TrackEntity(seller);

            _logger.LogInformation("Seller {SellerId} approving by admin", request.SellerId);

            return ResponseDTO.Success(new
            {
                request.SellerId,
                Status = SellerStatus.Active.ToString().ToLower(),
                seller.IsVerified,
                seller.VerifiedAt
            }, "Seller approved successfully");
        }
    }
}
