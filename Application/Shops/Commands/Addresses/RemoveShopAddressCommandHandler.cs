using Daco.Application.Common.Interfaces.Repositories;

namespace Daco.Application.Shops.Commands.Addresses
{
    public class RemoveShopAddressCommandHandler : IRequestHandler<RemoveShopAddressCommand, ResponseDTO>
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveShopAddressCommandHandler> _logger;

        public RemoveShopAddressCommandHandler(
            ISellerRepository sellerRepository,
            IShopRepository shopRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveShopAddressCommandHandler> logger)
        {
            _sellerRepository = sellerRepository;
            _shopRepository = shopRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(RemoveShopAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {UserId} remove address {AddressId}",
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

            address.SoftDelete();
            _unitOfWork.TrackEntity(shop);

            _logger.LogInformation(
                "Address {AddressId} removed by user {UserId}",
                request.AddressId, request.UserId);

            return ResponseDTO.Success(new
            {
                shopId = shop.Id,
                addressId = address.Id
            }, "Address removed successfully");
        }
    }
}
