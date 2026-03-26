namespace Daco.Application.Users.Commands.Banking
{
    public class AddBankAccountCommandHandler : IRequestHandler<AddBankAccountCommand, ResponseDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddBankAccountCommandHandler> _logger;

        public AddBankAccountCommandHandler(
            IUserRepository userRepository, 
            IBankAccountRepository bankAccountRepository, 
            IUnitOfWork unitOfWork, 
            ILogger<AddBankAccountCommandHandler> logger)
        {
            _userRepository = userRepository;
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ResponseDTO> Handle(AddBankAccountCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding bank account for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user is null)
                return ResponseDTO.Failure(ErrorCodes.UserErrors.NotFound, "User not found");

            var accounts = await _bankAccountRepository.GetByUserIdAsync(request.UserId, cancellationToken);

            if (accounts.Count >= 5)
                return ResponseDTO.Failure(ErrorCodes.BankAccountErrors.LimitExceeded, "Cannot have more than 5 active bank accounts");

            var isDuplicate = accounts.Any(b =>
                b.BankCode == request.BankCode.ToUpperInvariant() &&
                b.AccountNumber == request.AccountNumber);
            if (isDuplicate)
                return ResponseDTO.Failure(ErrorCodes.BankAccountErrors.AlreadyExists, "Bank account already exists");

            var isDefault = request.IsDefault || !accounts.Any();

            if (isDefault)
                foreach (var acc in accounts.Where(a => a.IsDefault))
                    acc.RemoveDefault();

            var bankAccount = BankAccount.Create(
                userId: request.UserId,
                bankCode: request.BankCode,
                bankName: request.BankName,
                accountNumber: request.AccountNumber,
                accountHolder: request.AccountHolder,
                isDefault: isDefault);

            await _bankAccountRepository.AddAsync(bankAccount, cancellationToken);

            _logger.LogInformation("Bank account added for user {UserId}", request.UserId);

            return ResponseDTO.Success(new
            {
                bankAccount.Id,
                bankAccount.BankCode,
                bankAccount.BankName,
                bankAccount.AccountNumber,
                bankAccount.AccountHolder,
                bankAccount.IsDefault,
                bankAccount.IsVerified
            }, "Bank account added successfully");
        }
    }
}
