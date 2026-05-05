namespace Daco.Application.Shops.Commands.Addresses
{
    public class UpdateShopAddressCommandHandler : IRequestHandler<UpdateShopAddressCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateShopAddressCommandHandler> _logger;

        public UpdateShopAddressCommandHandler(
            ISellerRepository sellerRepository,
            IShopRepository shopRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateShopAddressCommandHandler> logger)
        {
            _sellerRepository = sellerRepository;
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateShopAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "User {UserId} updating address {AddressId}",
                request.UserId, request.AddressId);

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

            try
            {
                shop.UpdateAddress(request.AddressId, ShopAddress.Create(
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
                    operatingHours: request.OperatingHours));
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDTO.Failure(ErrorCodes.AddressErrors.NotFound, ex.Message);
            }

            _unitOfWork.TrackEntity(shop);

            _logger.LogInformation(
                "Address {AddressId} updated for shop {ShopId}",
                request.AddressId, shop.Id);

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
                address.UpdatedAt
            }, "Address updated successfully");
        }
    }
}
