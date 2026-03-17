namespace Daco.Application.Users.Commands.Address
{
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, ResponseDTO>
    {
        private readonly IUserAddressRepository _addressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteAddressCommandHandler> _logger;

        public DeleteAddressCommandHandler(
            IUserAddressRepository addressRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<DeleteAddressCommandHandler> logger)
        {
            _addressRepository = addressRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting address {AddressId} for user {UserId}",
                request.AddressId, request.UserId);

            var address = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken);
            if (address is null || address.UserId != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.Address.NotFound, "Address not found");

            address.SoftDelete();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (address.IsDefault)
            {
                var others = await _addressRepository.GetByUserIdAsync(request.UserId, cancellationToken);
                var next = others.FirstOrDefault(a => a.Id != address.Id);
                next?.SetAsDefault();
            }    

            _logger.LogInformation("Address {AddressId} deleted for user {UserId}",
                request.AddressId, request.UserId);

            return ResponseDTO.Success(null, "Address deleted successfully");
        }
    }
}
