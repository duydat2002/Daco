namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public class RejectWithdrawalCommandHandler : IRequestHandler<RejectWithdrawalCommand, ResponseDTO>
    {
        private readonly IWithdrawalRepository _withdrawalRepository;
        private readonly ISellerWalletRepository _sellerWalletRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RejectWithdrawalCommandHandler> _logger;

        public RejectWithdrawalCommandHandler(
            IWithdrawalRepository withdrawalRepository,
            ISellerWalletRepository sellerWalletRepository,
            IUnitOfWork unitOfWork,
            ILogger<RejectWithdrawalCommandHandler> logger)
        {
            _withdrawalRepository = withdrawalRepository;
            _sellerWalletRepository = sellerWalletRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(RejectWithdrawalCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
            "Rejecting withdrawal {WithdrawalId} by admin {AdminId}",
            request.WithdrawalId, request.RejectedBy);

            var withdrawal = await _withdrawalRepository.GetByIdAsync(request.WithdrawalId, cancellationToken);
            if (withdrawal is null)
                return ResponseDTO.Failure(ErrorCodes.WithdrawalErrors.NotFound, "Withdrawal request not found");

            if (withdrawal.Status != WithdrawalStatus.Pending && withdrawal.Status != WithdrawalStatus.Approved)
                return ResponseDTO.Failure(
                    ErrorCodes.WithdrawalErrors.InvalidStatus,
                    $"Cannot reject a withdrawal with status: {withdrawal.Status}");

            var wallet = await _sellerWalletRepository.GetBySellerIdAsync(withdrawal.SellerId, cancellationToken);
            if (wallet is null)
                return ResponseDTO.Failure(ErrorCodes.WalletErrors.NotFound, "Seller wallet not found");

            // Hoàn lại số tiền đã bị trừ khi tạo request
            wallet.RefundWithdrawal(withdrawal.Amount);

            withdrawal.Reject(request.RejectedReason, request.AdminNote);

            _unitOfWork.TrackEntity(withdrawal);
            _unitOfWork.TrackEntity(wallet);

            _logger.LogInformation(
                "Withdrawal {WithdrawalId} rejected. Refunded {Amount} to seller {SellerId}",
                request.WithdrawalId, withdrawal.Amount, withdrawal.SellerId);

            return ResponseDTO.Success(new
            {
                withdrawal.Id,
                withdrawal.SellerId,
                withdrawal.Amount,
                Status = withdrawal.Status.ToString().ToLower(),
                withdrawal.RejectedReason,
                withdrawal.AdminNote
            }, "Withdrawal rejected and amount refunded to seller wallet");
        }
    }
}
