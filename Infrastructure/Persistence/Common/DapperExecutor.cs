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

        #region Stored Procedure
        /// <summary>
        /// Thực thi stored procedure và trả về single row hoặc null
        /// </summary>
        public async Task<T?> ExecuteProcedureSingleOrDefaultAsync<T>(
            string procedureName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Executing Procedure: {procedureName} - input: {JsonSerializer.Serialize(parameters)}");
            try
            {
                return await _session.Connection.QuerySingleOrDefaultAsync<T>(
                    procedureName,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing procedure: {procedureName}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi stored procedure và trả về danh sách kết quả
        /// </summary>
        public async Task<IEnumerable<T>> ExecuteProcedureQueryAsync<T>(
            string procedureName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Executing Procedure: {procedureName} - input: {JsonSerializer.Serialize(parameters)}");
            try
            {
                return await _session.Connection.QueryAsync<T>(
                    procedureName,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing procedure: {procedureName}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi stored procedure không trả về kết quả
        /// </summary>
        public async Task ExecuteProcedureAsync(
            string procedureName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Executing Procedure: {procedureName} - input: {JsonSerializer.Serialize(parameters)}");
            try
            {
                await _session.Connection.ExecuteAsync(
                    procedureName,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing procedure: {procedureName}");
                throw;
            }
        }
        #endregion

        #region Function
        /// <summary>
        /// Thực thi PostgreSQL function và trả về single row hoặc null
        /// </summary>
        public async Task<T?> ExecuteFunctionSingleOrDefaultAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Executing Function: {functionName} - input: {JsonSerializer.Serialize(parameters)}");
            try
            {
                var sql = BuildFunctionQuery(functionName, parameters);
                return await _session.Connection.QuerySingleOrDefaultAsync<T>(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing function: {functionName}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi PostgreSQL function và trả về danh sách kết quả
        /// </summary>
        public async Task<IEnumerable<T>> ExecuteFunctionQueryAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Executing Function: {functionName} - input: {JsonSerializer.Serialize(parameters)}");
            try
            {
                var sql = BuildFunctionQuery(functionName, parameters);
                return await _session.Connection.QueryAsync<T>(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing function: {functionName}");
                throw;
            }
        }

        /// <summary>
        /// Thực thi PostgreSQL function trả về scalar value
        /// </summary>
        public async Task<T?> ExecuteFunctionScalarAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Executing Function (Scalar): {functionName} - input: {JsonSerializer.Serialize(parameters)}");
            try
            {
                var sql = $"SELECT {functionName}({BuildParameterPlaceholders(parameters)})";
                return await _session.Connection.ExecuteScalarAsync<T>(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing function: {functionName}");
                throw;
            }
        }
        #endregion

        private string BuildFunctionQuery(string functionName, object? parameters)
        {
            return $"SELECT * FROM {functionName}({BuildParameterPlaceholders(parameters)})";
        }

        private string BuildParameterPlaceholders(object? parameters)
        {
            if (parameters == null) return string.Empty;

            IEnumerable<string> paramNames;
            if (parameters is DynamicParameters dynamicParams)
            {
                paramNames = dynamicParams.ParameterNames;
            }
            else
            {
                paramNames = parameters.GetType().GetProperties().Select(p => p.Name);
            }

            var paramList = paramNames.Select(name => $"{name} => @{name}");
            //var placeholders = properties.Select(p => $"@{p.Name}");
            return string.Join(", ", paramList);
        }

        /// <summary>
        /// Thực thi procedure/function với multiple result sets
        /// </summary>
        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> ExecuteMultipleAsync<T1, T2>(
            string name,
            object? parameters = null,
            bool isFunction = false,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug($"Executing {(isFunction ? "Function" : "Procedure")}: {name} - input: {JsonSerializer.Serialize(parameters)}");
            try
            {
                string sql;
                CommandType commandType;

                if (isFunction)
                {
                    sql = BuildFunctionQuery(name, parameters);
                    commandType = CommandType.Text;
                }
                else
                {
                    sql = name;
                    commandType = CommandType.StoredProcedure;
                }

                using var multi = await _session.Connection.QueryMultipleAsync(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: commandType);

                var result1 = await multi.ReadAsync<T1>();
                var result2 = await multi.ReadAsync<T2>();

                return (result1, result2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error executing: {name}");
                throw;
            }
        }
    }
}
