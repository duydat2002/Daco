namespace Daco.Infrastructure.Persistence.Common
{
    public static class RepositoryLogger
    {
        public static async Task<T?> ExecuteAsync<T>(
        ILogger logger,
        object? input,
        Func<Task<T?>> action,
        [CallerMemberName] string methodName = "")
        {
            logger.LogDebug("{Method} - Input: {@Input}", methodName, input);

            var result = await action();

            logger.LogDebug("{Method} - Output: {@Output}", methodName, result);

            return result;
        }

        public static async Task ExecuteAsync(
            ILogger logger,
            object? input,
            Func<Task> action,
            [CallerMemberName] string methodName = "")
        {
            logger.LogDebug("{Method} - Input: {@Input}", methodName, input);

            await action();

            logger.LogDebug("{Method} - Completed", methodName);
        }
    }
}
