namespace Daco.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PostgresEnumValueAttribute : Attribute
    {
        public string Value { get; }
        public PostgresEnumValueAttribute(string value) => Value = value;
    }
}
