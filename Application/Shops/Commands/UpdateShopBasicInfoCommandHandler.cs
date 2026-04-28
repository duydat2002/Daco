namespace Daco.Application.Shops.Commands
{
    public class UpdateShopBasicInfoCommandHandler : IRequestHandler<UpdateShopBasicInfoCommand, ResponseDTO>
    {
        private readonly IShopRepository _shopRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateShopBasicInfoCommandHandler> _logger;

        public UpdateShopBasicInfoCommandHandler(
            IShopRepository shopRepository,
            ISellerRepository sellerRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateShopBasicInfoCommandHandler> logger)
        {
            _shopRepository = shopRepository;
            _sellerRepository = sellerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateShopBasicInfoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Seller User {UserId} updating basic info for shop", request.UserId);

            var seller = await _sellerRepository.GetByUserIdAsync(
                request.UserId, cancellationToken);

            if (seller is null || !seller.IsActive)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Seller not found or not active");

            var shop = await _shopRepository.GetBySellerIdAsync(
                seller.Id, cancellationToken);

            if (shop is null)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Shop not found");

            if (shop.SellerId != seller.Id)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.Unauthorized, "You do not have permission to update this shop");

            if (shop.Status == ShopStatus.Closed)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Cannot update a closed shop");

            try
            {
                shop.UpdateProfile(
                    shopName: request.ShopName ?? shop.ShopName,
                    description: request.Description ?? shop.Description,
                    shopEmail: request.ShopEmail ?? shop.ShopEmail,
                    shopPhone: request.ShopPhone ?? shop.ShopPhone);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDTO.Failure(
                    ErrorCodes.ShopErrors.Suspended,
                    ex.Message);
            }
                
            _unitOfWork.TrackEntity(shop);

            _logger.LogInformation(
                "Shop {ShopId} basic info updated by seller {SellerId}",
                shop.Id, shop.SellerId);

            return ResponseDTO.Success(new
            {
                shop.Id,
                shop.ShopName,
                shop.Description,
                shop.ShopEmail,
                shop.ShopPhone,
                shop.UpdatedAt
            }, "Shop updated successfully");
        }
    }
}
