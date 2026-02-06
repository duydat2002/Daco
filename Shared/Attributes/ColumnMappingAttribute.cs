namespace Daco.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnMappingAttribute : Attribute
    {
        public string Name { get; }
        public ColumnMappingAttribute(string name) => Name = name.ToUpperInvariant();
    }
}
