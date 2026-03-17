namespace Daco.Application.Users.Commands.Banking
{
    public class DeleteBankAccountCommandHandler : IRequestHandler<DeleteBankAccountCommand, ResponseDTO>
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteBankAccountCommandHandler> _logger;

        public DeleteBankAccountCommandHandler(
            IBankAccountRepository bankAccountRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteBankAccountCommandHandler> logger)
        {
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(DeleteBankAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting bank account {BankAccountId} for user {UserId}",
                request.BankAccountId, request.UserId);

            var bankAccount = await _bankAccountRepository.GetByIdAsync(request.BankAccountId, cancellationToken);
            if (bankAccount is null || bankAccount.UserId != request.UserId)
                return ResponseDTO.Failure(ErrorCodes.BankAccount.NotFound, "Bank account not found");

            bankAccount.SoftDelete();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (bankAccount.IsDefault)
            {
                var others = await _bankAccountRepository.GetByUserIdAsync(request.UserId, cancellationToken);
                var next = others.FirstOrDefault(a => a.Id != bankAccount.Id);
                next?.SetAsDefault();
            }

            _logger.LogInformation("Bank account {BankAccountId} deleted for user {UserId}",
                request.BankAccountId, request.UserId);

            return ResponseDTO.Success(null, "Bank account deleted successfully");
        }
    }
}
