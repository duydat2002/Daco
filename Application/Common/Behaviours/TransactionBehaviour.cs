namespace Daco.Application.Common.Behaviours
{
    public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

        public TransactionBehaviour(
            IUnitOfWork unitOfWork,
            ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogInformation("Beginning transaction for {RequestName}", requestName);

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var response = await next();

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Completed transaction for {RequestName}", requestName);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in transaction for {RequestName}", requestName);

                await _unitOfWork.RollbackAsync(cancellationToken);

                throw;
            }
        }
    }
}
