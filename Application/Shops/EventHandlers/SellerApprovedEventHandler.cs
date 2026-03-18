namespace Daco.Application.Shops.EventHandlers
{
    public class SellerApprovedEventHandler : INotificationHandler<SellerApprovedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ILogger<SellerApprovedEventHandler> _logger;

        public SellerApprovedEventHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            ILogger<SellerApprovedEventHandler> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(SellerApprovedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Handling SellerApprovedEvent for Seller {SellerId}, User {UserId}",
                notification.SellerId,
                notification.UserId);

            try
            {
                var user = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

                if (user is null)
                {
                    _logger.LogWarning("User {UserId} not found when handling SellerApprovedEvent", notification.UserId);
                    return;
                }

                if (user.Email is not null)
                {
                    await _emailService.SendSellerApprovedAsync(
                        user.Email.Value,
                        user.Name ?? user.Username.Value,
                        cancellationToken);
                }

                _logger.LogInformation(
                    "User {UserId} marked as seller after approval",
                    notification.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error handling SellerApprovedEvent for Seller {SellerId}",
                    notification.SellerId);
            }
        }
    }
}
