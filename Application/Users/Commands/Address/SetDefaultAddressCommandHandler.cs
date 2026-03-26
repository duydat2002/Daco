namespace Daco.Application.Users.Commands.Address
{
    public class SetDefaultAddressCommandHandler : IRequestHandler<SetDefaultAddressCommand, ResponseDTO>
    {
        private readonly IUserAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetDefaultAddressCommandHandler> _logger;

        public SetDefaultAddressCommandHandler(
            IUserAddressRepository addressRepository,
            IUnitOfWork unitOfWork,
            ILogger<SetDefaultAddressCommandHandler> logger)
        {
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SetDefaultAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting default address {AddressId} for user {UserId}",
                request.AddressId, request.UserId);

            var address = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken);
            if (address is null || address.UserId != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.AddressErrors.NotFound, "Address not found");

            if (address.IsDefault)
                return ResponseDTO.Failure(ErrorCodes.AddressErrors.AlreadyDefault, "Address is already default");

            var addresses = await _addressRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            foreach (var addr in addresses.Where(a => a.IsDefault))
                addr.RemoveDefault();

            address.SetAsDefault();

            _logger.LogInformation("Default address set to {AddressId} for user {UserId}",
                request.AddressId, request.UserId);

            return ResponseDTO.Success(null, "Default address updated successfully");

        }
    }
}
