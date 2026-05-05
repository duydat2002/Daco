namespace Daco.Application.Shops.Commands.Addresses
{
    public class SetDefaultShopAddressCommandHandler : IRequestHandler<SetDefaultShopAddressCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetDefaultShopAddressCommandHandler> _logger;

        public SetDefaultShopAddressCommandHandler(
            ISellerRepository sellerRepository,
            IShopRepository shopRepository,
            IUnitOfWork unitOfWork,
            ILogger<SetDefaultShopAddressCommandHandler> logger)
        {
            _sellerRepository = sellerRepository;
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SetDefaultShopAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting default address {AddressId} for user {UserId}",
              request.AddressId, request.UserId);

            var seller = await _sellerRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (seller is null || !seller.IsActive)
                return ResponseDTO.Failure(ErrorCodes.SellerErrors.NotFound, "Seller not found or not active");

            var shop = await _shopRepository.GetBySellerIdAsync(seller.Id, cancellationToken);
            if (shop is null)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Shop not found");

            if (shop.Status == ShopStatus.Closed)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.Closed, "Cannot update a closed shop");

            var address = shop.ShopAddresses
                .FirstOrDefault(a => a.Id == request.AddressId && !a.IsDeleted);

            if (address is null)
                return ResponseDTO.Failure(ErrorCodes.AddressErrors.NotFound, "Address not found");

            foreach (var acc in shop.ShopAddresses.Where(a => a.IsDefault))
                acc.RemoveDefault();

            address.SetAsDefault();
            _unitOfWork.TrackEntity(shop);

            _logger.LogInformation("Default address set to {AddressId} for user {UserId}",
                request.AddressId, request.UserId);

            return ResponseDTO.Success(null, "Default address updated successfully");
        }
    }
}
