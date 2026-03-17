namespace Daco.Application.Users.Commands.Banking
{
    public class SetDefaultBankAccountCommandHandler : IRequestHandler<SetDefaultBankAccountCommand, ResponseDTO>
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SetDefaultBankAccountCommandHandler> _logger;

        public SetDefaultBankAccountCommandHandler(
            IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork,
            ILogger<SetDefaultBankAccountCommandHandler> logger)
        {
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(SetDefaultBankAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Setting default bank account {BankAccountId} for user {UserId}",
               request.BankAccountId, request.UserId);

            var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);
            if (bankAccount is null || bankAccount.UserId != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.BankAccount.NotFound, "Bank account not found");

            if (bankAccount.IsDefault)
                return ResponseDTO.Failure(ErrorCodes.BankAccount.AlreadyDefault, "Bank account is already default");

            var bankAccounts = await _bankAccountRepository.GetByUserIdAsync(request.UserId, cancellationToken);
            foreach (var acc in bankAccounts.Where(a => a.IsDefault))
                acc.RemoveDefault();

            bankAccount.SetAsDefault();

            _logger.LogInformation("Default bank account set to {BankAccountId} for user {UserId}",
                request.BankAccountId, request.UserId);

            return ResponseDTO.Success(null, "Default bank account updated successfully");
        }
    }
}
