namespace Daco.Application.Shops.Commands
{
    public class UpdateShopLogoCommandHandler : IRequestHandler<UpdateShopLogoCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateShopLogoCommandHandler> _logger;

        public UpdateShopLogoCommandHandler(
            ISellerRepository sellerRepository,
            IShopRepository shopRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork,
            ILogger<UpdateShopLogoCommandHandler> logger)
        {
            _sellerRepository = sellerRepository;
            _shopRepository = shopRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateShopLogoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} updating logo for shop", request.UserId);

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

            var oldLogoUrl = shop.ShopLogo;

            var newLogoUrl = await _fileStorageService.UploadShopLogoAsync(
                request.UserId,
                shop.Id,
                request.FileStream,
                request.FileName,
                request.ContentType,
                cancellationToken);

            shop.UpdateLogo(newLogoUrl);
            _unitOfWork.TrackEntity(shop);

            if (!string.IsNullOrEmpty(oldLogoUrl))
            {
                try
                {
                    await _fileStorageService.DeleteAsync(oldLogoUrl, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete old avatar {OldLogoUrl}", oldLogoUrl);
                }
            }

            _logger.LogInformation("Logo updated for shop {ShopId}", shop.Id);

            return ResponseDTO.Success(new { LogoUrl = newLogoUrl }, "Shop logo updated successfully");
        }
    }
}
