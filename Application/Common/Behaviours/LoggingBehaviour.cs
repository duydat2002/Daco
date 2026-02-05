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

            _logger.LogInformation($"[START] {requestName} - RequestId: {requestId} - Request: {JsonSerializer.Serialize(request)}");

            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();

                stopwatch.Stop();

                _logger.LogInformation($"[END] {requestName} - RequestId: {requestId} - Elapsed: {stopwatch.ElapsedMilliseconds}ms - Success: {IsSuccessResponse(response)}");

                if (stopwatch.ElapsedMilliseconds > 3000)
                {
                    _logger.LogWarning($"[SLOW REQUEST] {requestName} took {stopwatch.ElapsedMilliseconds}ms");
                }

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                _logger.LogError(ex, $"[ERROR] {requestName} - RequestId: {requestId} - Elapsed: {stopwatch.ElapsedMilliseconds}ms - Error: {ex.Message}");

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
