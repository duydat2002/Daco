namespace Daco.Application.Users.Commands.Address
{
    public class AddAddressCommandHandler : IRequestHandler<AddAddressCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddAddressCommandHandler> _logger;

        public AddAddressCommandHandler(
            IUserRepository userRepository,
            IUserAddressRepository addressRepository,
            IUnitOfWork unitOfWork,
            ILogger<AddAddressCommandHandler> logger)
        {
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AddAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding address for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            var addresses = await _addressRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (addresses.Count >= 10)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.AddressLimitExceeded, "Cannot have more than 10 active addresses");

            if (request.IsDefault)
            {
                foreach (var addr in addresses.Where(a => a.IsDefault))
                    addr.RemoveDefault();
            }

            var address = UserAddress.Create(
                userId: request.UserId,
                recipientName: request.RecipientName,
                recipientPhone: request.RecipientPhone,
                city: request.City,
                district: request.District,
                ward: request.Ward,
                addressDetail: request.AddressDetail,
                label: request.Label,
                addressType: request.AddressType,
                latitude: request.Latitude,
                longitude: request.Longitude,
                isDefault: request.IsDefault || !addresses.Any());

            await _addressRepository.AddAsync(address, cancellationToken);

            _logger.LogInformation("Address {AddressId} added for user {UserId}", address.Id, request.UserId);

            return ResponseDTO.Success(new
            {
                address.Id,
                address.RecipientName,
                address.RecipientPhone,
                address.City,
                address.District,
                address.Ward,
                address.AddressDetail,
                address.Label,
                address.AddressType,
                address.IsDefault
            }, "Address added successfully");
        }
    }
}
