namespace Daco.Application.Users.Commands.Banking
{
    public class UpdateBankAccountCommandHandler : IRequestHandler<UpdateBankAccountCommand, ResponseDTO>
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateBankAccountCommandHandler> _logger;

        public UpdateBankAccountCommandHandler(
            IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateBankAccountCommandHandler> logger)
        {
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(UpdateBankAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating bank account {BankAccountId} for user {UserId}",
                request.BankAccountId, request.UserId);

            var account = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);
            if (account is null || account.UserId != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.BankAccount.NotFound, "Bank account not found");

            var accounts = await _bankAccountRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            var isDuplicate = accounts.Any(b =>
                b.Id != request.BankAccountId &&
                b.BankCode == request.BankCode.ToUpperInvariant() &&
                b.AccountNumber == request.AccountNumber);

            if (isDuplicate)
                return ResponseDTO.Failure(ErrorCodes.BankAccount.AlreadyExists, "Bank account already exists");

            account.Update(
                bankCode: request.BankCode,
                bankName: request.BankName,
                accountNumber: request.AccountNumber,
                accountHolder: request.AccountHolder);

            _logger.LogInformation("Bank account {BankAccountId} updated for user {UserId}",
                request.BankAccountId, request.UserId);

            return ResponseDTO.Success(new
            {
                account.Id,
                account.BankCode,
                account.BankName,
                account.AccountNumber,
                account.AccountHolder,
                account.IsDefault,
                account.IsVerified
            }, "Bank account updated successfully");
        }
    }
}
