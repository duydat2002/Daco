namespace Daco.Application.Shops.Commands
{
    public class OnboardSellerCommandHandler : IRequestHandler<OnboardSellerCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISellerRepository _sellerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OnboardSellerCommand> _logger;

        public OnboardSellerCommandHandler(
            IUserRepository userRepository,
            ISellerRepository sellerRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<OnboardSellerCommand> logger)
        {
            _userRepository = userRepository;
            _sellerRepository = sellerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(OnboardSellerCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Seller onboarding for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            if (user.Status != UserStatus.Active)
                return ResponseDTO.Failure(ErrorCodes.AuthErrors.AccountSuspended, "Account is not active");

            var existing = await _sellerRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            if (existing is not null)
            {
                _logger.LogInformation("Seller record already exists for user {UserId}, returning existing", request.UserId);
                return ResponseDTO.Success(new
                {
                    existing.Id,
                    existing.UserId,
                    existing.BusinessType,
                    Status = existing.Status.ToString().ToLower(),
                    existing.IsVerified,
                    KycCompleted = existing.BusinessType != null
                }, "Seller record already exists");
            }

            var seller = Seller.Onboard(request.UserId);

            await _sellerRepository.AddAsync(seller, cancellationToken);
            _unitOfWork.TrackEntity(seller);

            _logger.LogInformation("Seller record created: {SellerId} for user {UserId}",
                seller.Id, request.UserId);

            return ResponseDTO.Success(new
            {
                seller.Id,
                seller.UserId,
                seller.BusinessType,
                Status = seller.Status.ToString().ToLower(),
                seller.IsVerified,
                KycCompleted = seller.BusinessType != null
            }, "Seller onboarded successfully");
        }
    }
}
