namespace Daco.Infrastructure.Persistence.Common
{
    public class NpgsqlDbSession : IDbSession
    {
        private readonly NpgsqlDataSource _dataSource;
        private NpgsqlConnection? _connection;
        private NpgsqlTransaction? _transaction;
        private bool _disposed;

        public NpgsqlDbSession(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public IDbConnection Connection
        {
            get
            {
                if (_connection == null || _connection.State == ConnectionState.Closed)
                {
                    _connection = _dataSource.CreateConnection();
                    _connection.Open();
                }
                return _connection;
            }
        }

        public IDbTransaction? Transaction => _transaction;

        public async Task OpenAsync(CancellationToken cancellationToken = default)
        {
            if (_connection == null || _connection.State == ConnectionState.Closed)
            {
                _connection = await _dataSource.OpenConnectionAsync(cancellationToken);
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await OpenAsync(cancellationToken);

            if (_transaction != null)
                return;

            _transaction = await _connection!.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                throw new InvalidOperationException("No active transaction to commit");

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
                return;

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }

            _disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            if (_transaction != null)
                await _transaction.DisposeAsync();

            if (_connection != null)
                await _connection.DisposeAsync();

            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
