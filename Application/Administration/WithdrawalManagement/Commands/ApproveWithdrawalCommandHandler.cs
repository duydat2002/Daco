namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public class ApproveWithdrawalCommandHandler : IRequestHandler<ApproveWithdrawalCommand, ResponseDTO>
    {
        private readonly IWithdrawalRepository _withdrawalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ApproveWithdrawalCommandHandler(
            IWithdrawalRepository withdrawalRepository,
            IUnitOfWork unitOfWork,
            ILogger logger)
        {
            _withdrawalRepository = withdrawalRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(ApproveWithdrawalCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
            "Approving withdrawal {WithdrawalId} by admin {AdminId}",
            request.WithdrawalId, request.ApprovedBy);

            var withdrawal = await _withdrawalRepository.GetByIdAsync(request.WithdrawalId, cancellationToken);
            if (withdrawal is null)
                return ResponseDTO.Failure(ErrorCodes.WithdrawalErrors.NotFound, "Withdrawal request not found");

            if (withdrawal.Status != WithdrawalStatus.Pending)
                return ResponseDTO.Failure(
                    ErrorCodes.WithdrawalErrors.InvalidStatus,
                    $"Only pending withdrawals can be approved. Current status: {withdrawal.Status}");

            withdrawal.Approve(request.ApprovedBy, request.AdminNote);

            _unitOfWork.TrackEntity(withdrawal);

            _logger.LogInformation(
                "Withdrawal {WithdrawalId} approved by admin {AdminId}",
                request.WithdrawalId, request.ApprovedBy);

            return ResponseDTO.Success(new
            {
                withdrawal.Id,
                withdrawal.SellerId,
                withdrawal.Amount,
                withdrawal.NetAmount,
                Status = withdrawal.Status.ToString().ToLower(),
                withdrawal.ApprovedAt,
                withdrawal.AdminNote
            }, "Withdrawal approved successfully");
        }
    }
}
