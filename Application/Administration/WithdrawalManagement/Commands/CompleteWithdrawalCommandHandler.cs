namespace Daco.Application.Administration.WithdrawalManagement.Commands
{
    public class CompleteWithdrawalCommandHandler : IRequestHandler<CompleteWithdrawalCommand, ResponseDTO>
    {
        private readonly IWithdrawalRepository _withdrawalRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CompleteWithdrawalCommandHandler> _logger;

        public CompleteWithdrawalCommandHandler(
            IWithdrawalRepository withdrawalRepository,
            IUnitOfWork unitOfWork,
            ILogger<CompleteWithdrawalCommandHandler> logger)
        {
            _withdrawalRepository = withdrawalRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(CompleteWithdrawalCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
            "Completing withdrawal {WithdrawalId} by admin {AdminId}, txCode: {TxCode}",
            request.WithdrawalId, request.CompletedBy, request.TransactionCode);

            var withdrawal = await _withdrawalRepository.GetByIdAsync(request.WithdrawalId, cancellationToken);
            if (withdrawal is null)
                return ResponseDTO.Failure(ErrorCodes.WithdrawalErrors.NotFound, "Withdrawal request not found");

            if (withdrawal.Status != WithdrawalStatus.Approved && withdrawal.Status != WithdrawalStatus.Processing)
                return ResponseDTO.Failure(
                    ErrorCodes.WithdrawalErrors.InvalidStatus,
                    $"Only approved or processing withdrawals can be completed. Current status: {withdrawal.Status}");

            withdrawal.Complete(request.TransactionCode, request.AdminNote);

            _unitOfWork.TrackEntity(withdrawal);

            _logger.LogInformation(
                "Withdrawal {WithdrawalId} completed. TransactionCode: {TxCode}",
                request.WithdrawalId, request.TransactionCode);

            return ResponseDTO.Success(new
            {
                withdrawal.Id,
                withdrawal.SellerId,
                withdrawal.Amount,
                withdrawal.NetAmount,
                Status = withdrawal.Status.ToString().ToLower(),
                withdrawal.TransactionCode,
                withdrawal.CompletedAt
            }, "Withdrawal completed successfully");
        }
    }
}
