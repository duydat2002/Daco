namespace Daco.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

        public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var requestId = Guid.NewGuid().ToString();

            _logger.LogInformation(
                "[START] {RequestName} - RequestId: {RequestId} - Request: {Request}",
                requestName,
                requestId,
                JsonSerializer.Serialize(request)
            );

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation(
                    "[END] {RequestName} - RequestId: {RequestId} - Elapsed: {Elapsed}ms - Success: {Success}",
                    requestName,
                    requestId,
                    stopwatch.ElapsedMilliseconds,
                    IsSuccessResponse(response)
                );

                if (stopwatch.ElapsedMilliseconds > 3000)
                {
                    _logger.LogWarning(
                        "[SLOW REQUEST] {RequestName} took {Elapsed}ms",
                        requestName,
                        stopwatch.ElapsedMilliseconds
                    );
                }

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(
                    ex,
                    "[ERROR] {RequestName} - RequestId: {RequestId} - Elapsed: {Elapsed}ms - Error: {Message}",
                    requestName, 
                    requestId,
                    stopwatch.ElapsedMilliseconds,
                    ex.Message
                );

                throw;
            }
        }

        private static bool IsSuccessResponse(TResponse response)
        {
            var isSuccessProperty = response?.GetType().GetProperty("IsSuccess");
            if (isSuccessProperty != null)
            {
                return (bool)(isSuccessProperty.GetValue(response) ?? false);
            }

            return true;
        }
    }
}
