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
        /// Thực thi PROCEDURE không trả về rows.
        /// Dùng cho INSERT/UPDATE/DELETE procedure, thường kèm OUT parameters (o_code, o_message).
        /// Generate SQL: CALL procedure_name(@p1, @p2, ...)
        /// </summary>
        public async Task ExecuteProcedureAsync(
            string procedureName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing Procedure: {Name}", procedureName);
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
                _logger.LogError(ex, "Error executing procedure: {Name}", procedureName);
                throw;
            }
        }

        /// <summary>
        /// Thực thi PROCEDURE và đọc kết quả dưới dạng single row.
        /// Dùng khi procedure có nhiều OUT params cần map vào object T.
        /// Generate SQL: CALL procedure_name(@p1, @p2, ...)
        /// </summary>
        public async Task<T?> ExecuteProcedureSingleOrDefaultAsync<T>(
            string procedureName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing Procedure (SingleOrDefault): {Name}", procedureName);
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
                _logger.LogError(ex, "Error executing procedure: {Name}", procedureName);
                throw;
            }
        }

        #endregion

        #region Function

        /// <summary>
        /// Thực thi FUNCTION trả về danh sách rows (RETURNS TABLE hoặc RETURNS SETOF).
        /// Generate SQL: SELECT * FROM function_name(p1 => @p1, p2 => @p2)
        /// </summary>
        public async Task<IEnumerable<T>> ExecuteFunctionQueryAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing Function (Query): {Name}", functionName);
            try
            {
                var sql = BuildFunctionSql(functionName, parameters);
                return await _session.Connection.QueryAsync<T>(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing function: {Name}", functionName);
                throw;
            }
        }

        /// <summary>
        /// Thực thi FUNCTION trả về single row hoặc null.
        /// Generate SQL: SELECT * FROM function_name(p1 => @p1, p2 => @p2)
        /// </summary>
        public async Task<T?> ExecuteFunctionSingleOrDefaultAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing Function (SingleOrDefault): {Name}", functionName);
            try
            {
                var sql = BuildFunctionSql(functionName, parameters);
                return await _session.Connection.QuerySingleOrDefaultAsync<T>(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing function: {Name}", functionName);
                throw;
            }
        }

        /// <summary>
        /// Thực thi FUNCTION trả về scalar value (single value, không phải row).
        /// Generate SQL: SELECT function_name(p1 => @p1, p2 => @p2)
        /// </summary>
        public async Task<T?> ExecuteFunctionScalarAsync<T>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing Function (Scalar): {Name}", functionName);
            try
            {
                var sql = BuildFunctionScalarSql(functionName, parameters);
                return await _session.Connection.ExecuteScalarAsync<T>(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing function (scalar): {Name}", functionName);
                throw;
            }
        }

        #endregion

        #region Multiple Result Sets

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> ExecuteFunctionMultipleAsync<T1, T2>(
            string functionName,
            object? parameters = null,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug("Executing Function (Multiple): {Name}", functionName);
            try
            {
                var sql = BuildFunctionSql(functionName, parameters);
                using var multi = await _session.Connection.QueryMultipleAsync(
                    sql,
                    parameters,
                    _session.Transaction,
                    commandType: CommandType.Text);

                var result1 = await multi.ReadAsync<T1>();
                var result2 = await multi.ReadAsync<T2>();
                return (result1, result2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing function (multiple): {Name}", functionName);
                throw;
            }
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Build SQL cho FUNCTION trả về rows.
        /// Format: SELECT * FROM fn_name(param1 => @param1, param2 => @param2)
        ///
        /// Named parameter syntax (name => @name) giúp PostgreSQL resolve đúng overload
        /// và không cần quan tâm đến thứ tự tham số.
        ///
        /// Lọc bỏ OUT parameters (prefix "o_") để không truyền nhầm vào function args.
        /// </summary>
        private static string BuildFunctionSql(string functionName, object? parameters)
        {
            return $"SELECT * FROM {functionName}({BuildNamedParams(parameters)})";
        }

        /// <summary>
        /// Build SQL cho FUNCTION trả về scalar.
        /// Format: SELECT fn_name(param1 => @param1, param2 => @param2)
        /// </summary>
        private static string BuildFunctionScalarSql(string functionName, object? parameters)
        {
            return $"SELECT {functionName}({BuildNamedParams(parameters)})";
        }

        private static string BuildNamedParams(object? parameters)
        {
            if (parameters == null) return string.Empty;

            IEnumerable<string> names = parameters is DynamicParameters dp
                ? dp.ParameterNames
                : parameters.GetType().GetProperties().Select(p => p.Name);

            // Lọc bỏ output params và ReturnValue — chúng không phải input của function
            var inputNames = names.Where(n =>
                !n.StartsWith("o_", StringComparison.OrdinalIgnoreCase) &&
                !n.Equals("ReturnValue", StringComparison.OrdinalIgnoreCase));

            return string.Join(", ", inputNames.Select(n => $"{n} => @{n}"));
        }

        #endregion


        //#region Stored Procedure
        ///// <summary>
        ///// Thực thi stored procedure và trả về single row hoặc null
        ///// </summary>
        //public async Task<T?> ExecuteProcedureSingleOrDefaultAsync<T>(
        //    string procedureName,
        //    object? parameters = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Executing Procedure: {procedureName} - input: {JsonSerializer.Serialize(parameters)}");
        //    try
        //    {
        //        return await _session.Connection.QuerySingleOrDefaultAsync<T>(
        //            procedureName,
        //            parameters,
        //            _session.Transaction,
        //            commandType: CommandType.StoredProcedure);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error executing procedure: {procedureName}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Thực thi stored procedure và trả về danh sách kết quả
        ///// </summary>
        //public async Task<IEnumerable<T>> ExecuteProcedureQueryAsync<T>(
        //    string procedureName,
        //    object? parameters = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Executing Procedure: {procedureName} - input: {JsonSerializer.Serialize(parameters)}");
        //    try
        //    {
        //        return await _session.Connection.QueryAsync<T>(
        //            procedureName,
        //            parameters,
        //            _session.Transaction,
        //            commandType: CommandType.StoredProcedure);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error executing procedure: {procedureName}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Thực thi stored procedure không trả về kết quả
        ///// </summary>
        //public async Task ExecuteProcedureAsync(
        //    string procedureName,
        //    object? parameters = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Executing Procedure: {procedureName} - input: {JsonSerializer.Serialize(parameters)}");
        //    try
        //    {
        //        await _session.Connection.ExecuteAsync(
        //            procedureName,
        //            parameters,
        //            _session.Transaction,
        //            commandType: CommandType.StoredProcedure);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error executing procedure: {procedureName}");
        //        throw;
        //    }
        //}
        //#endregion

        //#region Function
        ///// <summary>
        ///// Thực thi PostgreSQL function và trả về single row hoặc null
        ///// </summary>
        //public async Task<T?> ExecuteFunctionSingleOrDefaultAsync<T>(
        //    string functionName,
        //    object? parameters = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Executing Function: {functionName} - input: {JsonSerializer.Serialize(parameters)}");
        //    try
        //    {
        //        var sql = BuildFunctionQuery(functionName, parameters);
        //        return await _session.Connection.QuerySingleOrDefaultAsync<T>(
        //            sql,
        //            parameters,
        //            _session.Transaction,
        //            commandType: CommandType.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error executing function: {functionName}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Thực thi PostgreSQL function và trả về danh sách kết quả
        ///// </summary>
        //public async Task<IEnumerable<T>> ExecuteFunctionQueryAsync<T>(
        //    string functionName,
        //    object? parameters = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Executing Function: {functionName} - input: {JsonSerializer.Serialize(parameters)}");
        //    try
        //    {
        //        var sql = BuildFunctionQuery(functionName, parameters);
        //        return await _session.Connection.QueryAsync<T>(
        //            sql,
        //            parameters,
        //            _session.Transaction,
        //            commandType: CommandType.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error executing function: {functionName}");
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Thực thi PostgreSQL function trả về scalar value
        ///// </summary>
        //public async Task<T?> ExecuteFunctionScalarAsync<T>(
        //    string functionName,
        //    object? parameters = null,
        //    CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Executing Function (Scalar): {functionName} - input: {JsonSerializer.Serialize(parameters)}");
        //    try
        //    {
        //        var sql = $"SELECT {functionName}({BuildParameterPlaceholders(parameters)})";
        //        return await _session.Connection.ExecuteScalarAsync<T>(
        //            sql,
        //            parameters,
        //            _session.Transaction,
        //            commandType: CommandType.Text);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error executing function: {functionName}");
        //        throw;
        //    }
        //}
        //#endregion

        //private string BuildFunctionQuery(string functionName, object? parameters)
        //{
        //    return $"SELECT * FROM {functionName}({BuildParameterPlaceholders(parameters)})";
        //}

        //private string BuildParameterPlaceholders(object? parameters)
        //{
        //    if (parameters == null) return string.Empty;

        //    IEnumerable<string> paramNames;
        //    if (parameters is DynamicParameters dynamicParams)
        //    {
        //        paramNames = dynamicParams.ParameterNames;
        //    }
        //    else
        //    {
        //        paramNames = parameters.GetType().GetProperties().Select(p => p.Name);
        //    }

        //    var placeholders = paramNames.Select(name => $"@{name}");

        //    return string.Join(", ", placeholders);
        //}

        ///// <summary>
        ///// Thực thi procedure/function với multiple result sets
        ///// </summary>
        //public async Task<(IEnumerable<T1>, IEnumerable<T2>)> ExecuteMultipleAsync<T1, T2>(
        //    string name,
        //    object? parameters = null,
        //    bool isFunction = false,
        //    CancellationToken cancellationToken = default)
        //{
        //    _logger.LogDebug($"Executing {(isFunction ? "Function" : "Procedure")}: {name} - input: {JsonSerializer.Serialize(parameters)}");
        //    try
        //    {
        //        string sql;
        //        CommandType commandType;

        //        if (isFunction)
        //        {
        //            sql = BuildFunctionQuery(name, parameters);
        //            commandType = CommandType.Text;
        //        }
        //        else
        //        {
        //            sql = name;
        //            commandType = CommandType.StoredProcedure;
        //        }

        //        using var multi = await _session.Connection.QueryMultipleAsync(
        //            sql,
        //            parameters,
        //            _session.Transaction,
        //            commandType: commandType);

        //        var result1 = await multi.ReadAsync<T1>();
        //        var result2 = await multi.ReadAsync<T2>();

        //        return (result1, result2);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error executing: {name}");
        //        throw;
        //    }
        //}
    }
}
