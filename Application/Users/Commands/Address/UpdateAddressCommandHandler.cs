namespace Daco.Application.Users.Commands.Address
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, ResponseDTO>
    {
        private readonly IUserAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateAddressCommandHandler> _logger;

        public UpdateAddressCommandHandler(
            IUserAddressRepository addressRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateAddressCommandHandler> logger)
        {
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating address {AddressId} for user {UserId}",
                request.AddressId, request.UserId);

            var address = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken);
            if (address is null)
                return ResponseDTO.Failure(ErrorCodes.Address.NotFound, "Address not found");

            if (address.UserId != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.Address.NotFound, "Address not found");

            address.Update(
                recipientName: request.RecipientName,
                recipientPhone: request.RecipientPhone,
                city: request.City,
                district: request.District,
                ward: request.Ward,
                addressDetail: request.AddressDetail,
                label: request.Label,
                latitude: request.Latitude,
                longitude: request.Longitude);

            _logger.LogInformation("Address {AddressId} updated for user {UserId}",
                request.AddressId, request.UserId);

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
                address.IsDefault
            }, "Address updated successfully");
        }
    }
}
