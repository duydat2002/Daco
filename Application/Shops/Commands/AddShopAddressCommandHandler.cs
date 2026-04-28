namespace Daco.Application.Shops.Commands
{
    public class AddShopAddressCommandHandler : IRequestHandler<AddShopAddressCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddShopAddressCommandHandler> _logger;

        public AddShopAddressCommandHandler(
            ISellerRepository sellerRepository,
            IShopRepository shopRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddShopAddressCommandHandler> logger)
        {
            _sellerRepository = sellerRepository;
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AddShopAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "User {UserId} adding {AddressType} address to shop",
                request.UserId, request.AddressType);

            var seller = await _sellerRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (seller is null || !seller.IsActive)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Seller not found or not active");

            var shop = await _shopRepository.GetBySellerIdAsync(seller.Id, cancellationToken);

            if (shop is null)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Shop not found");

            if (shop.Status == ShopStatus.Closed)
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.NotFound, "Cannot add address to a closed shop");

            var address = ShopAddress.Create(
                shopId: shop.Id,
                addressType: request.AddressType,
                contactName: request.ContactName,
                contactPhone: request.ContactPhone,
                city: request.City,
                district: request.District,
                ward: request.Ward,
                addressDetail: request.AddressDetail,
                latitude: request.Latitude,
                longitude: request.Longitude,
                label: request.Label,
                googlePlaceId: request.GooglePlaceId,
                operatingHours: request.OperatingHours,
                isDefault: request.IsDefault);

            try
            {
                shop.AddAddress(address);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDTO.Failure(ErrorCodes.ShopErrors.AddressLimitExceeded, ex.Message);
            }

            _unitOfWork.TrackEntity(shop);

            _logger.LogInformation("Address {AddressId} added to shop {ShopId}", address.Id, shop.Id);

            return ResponseDTO.Success(new
            {
                address.Id,
                address.ShopId,
                AddressType = address.AddressType.ToString().ToLower(),
                address.ContactName,
                address.ContactPhone,
                address.City,
                address.District,
                address.Ward,
                address.AddressDetail,
                address.Latitude,
                address.Longitude,
                address.Label,
                address.IsDefault,
                address.CreatedAt
            }, "Address added successfully");
        }
    }
}
