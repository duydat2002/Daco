namespace Daco.Infrastructure.Persistence.Common
{
    public class DapperExecutor
    {
        private readonly IDbSession _session;
        private readonly ILogger<DapperExecutor> _logger;

        public DapperExecutor(IDbSession session, ILogger<DapperExecutor> logger)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(
            string storedProcedure,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing SP: {StoredProcedure}", storedProcedure);

            try
            {
                return await _session.Connection.QuerySingleOrDefaultAsync<T>(
                    storedProcedure,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        public async Task<T> QuerySingleAsync<T>(
            string storedProcedure,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing SP: {StoredProcedure}", storedProcedure);

            try
            {
                return await _session.Connection.QuerySingleAsync<T>(
                    storedProcedure,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(
            string storedProcedure,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing SP: {StoredProcedure}", storedProcedure);

            try
            {
                return await _session.Connection.QueryAsync<T>(
                    storedProcedure,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        public async Task<int> ExecuteAsync(
            string storedProcedure,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing SP: {StoredProcedure}", storedProcedure);

            try
            {
                return await _session.Connection.ExecuteAsync(
                    storedProcedure,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        public async Task<T?> ExecuteScalarAsync<T>(
            string storedProcedure,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing SP: {StoredProcedure}", storedProcedure);

            try
            {
                return await _session.Connection.ExecuteScalarAsync<T>(
                    storedProcedure,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        // ⭐ Multiple result sets
        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> QueryMultipleAsync<T1, T2>(
            string storedProcedure,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing SP with multiple results: {StoredProcedure}", storedProcedure);

            try
            {
                using var multi = await _session.Connection.QueryMultipleAsync(
                    storedProcedure,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);

                var result1 = await multi.ReadAsync<T1>();
                var result2 = await multi.ReadAsync<T2>();

                return (result1, result2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> QueryMultipleAsync<T1, T2, T3>(
            string storedProcedure,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing SP with multiple results: {StoredProcedure}", storedProcedure);

            try
            {
                using var multi = await _session.Connection.QueryMultipleAsync(
                    storedProcedure,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);

                var result1 = await multi.ReadAsync<T1>();
                var result2 = await multi.ReadAsync<T2>();
                var result3 = await multi.ReadAsync<T3>();

                return (result1, result2, result3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }
    }
}
