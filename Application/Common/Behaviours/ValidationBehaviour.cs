namespace Daco.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ResponseDTO, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;

        public ValidationBehaviour(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                _logger.LogWarning(
                    "Validation failed for {RequestName}. Errors: {Errors}",
                    typeof(TRequest).Name,
                    string.Join("; ", failures.Select(f => f.ErrorMessage))
                );

                var response = new TResponse
                {
                    IsSuccess = false,
                    Code = ErrorCodes.Validation.Failed,
                    Message = "Validation failed",
                    Data = new
                    {
                        Errors = failures
                            .GroupBy(f => f.PropertyName)
                            .ToDictionary(
                                g => g.Key,
                                g => g.Select(f => f.ErrorMessage).ToArray()
                            )
                    }
                };

                return response;
            }

            return await next();
        }
    }
}
