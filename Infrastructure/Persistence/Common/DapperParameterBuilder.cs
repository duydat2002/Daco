namespace Daco.Infrastructure.Persistence.Common
{
    public class DapperParameterBuilder
    {
        private readonly DynamicParameters _parameters = new();

        public DapperParameterBuilder Add(string name, object? value, DbType? dbType = null)
        {
            _parameters.Add(name, value, dbType);
            return this;
        }

        public DapperParameterBuilder AddOutput(string name, DbType dbType)
        {
            _parameters.Add(name, dbType: dbType, direction: ParameterDirection.Output);
            return this;
        }

        public DapperParameterBuilder AddReturnValue()
        {
            _parameters.Add("@ReturnValue", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            return this;
        }

        public DynamicParameters Build() => _parameters;

        public T GetOutputValue<T>(string name)
        {
            return _parameters.Get<T>(name);
        }

        public int GetReturnValue()
        {
            return _parameters.Get<int>("@ReturnValue");
        }
    }
}
